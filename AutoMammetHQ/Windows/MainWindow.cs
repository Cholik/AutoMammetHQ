using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AutoMammetHQ.Model;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace AutoMammetHQ.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Reader reader;

    private DateTime nextSupplyAndDemandUpdateTime = DateTime.UtcNow;
    private bool schedulesUpdated = false;

    private Handicraft[] handicrafts;
    private ScheduleHandler scheduleHandler;
    private IEnumerable<Schedule>? schedules = null;
    private int cycle = 0;

    public MainWindow(Plugin plugin) : base(
        "AutoMammet (HQ)", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(400, 50),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.reader = new Reader(plugin);
    }

    public void Dispose()
    {
        Dalamud.ClientState.ClientLanguage.ToString();
    }

    public override void Draw()
    {
        if (DateTime.UtcNow >= nextSupplyAndDemandUpdateTime)
        {
            if (reader.IsSupplyAndDemandAvailable())
            {
                var supplyAndDemand = reader.GetSupplyAndDemand();
                handicrafts = reader.Handicrafts;

                scheduleHandler = new ScheduleHandler(handicrafts, supplyAndDemand);
                schedules = scheduleHandler.GetSchedules();
                cycle = scheduleHandler.GetCycle(DateTime.UtcNow) + 1;

                schedulesUpdated = true;

                nextSupplyAndDemandUpdateTime = DateTime.SpecifyKind(
                    new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 8, 0, 0), DateTimeKind.Utc);

                if (DateTime.UtcNow.TimeOfDay > new TimeSpan(8, 0, 0))
                {
                    nextSupplyAndDemandUpdateTime = nextSupplyAndDemandUpdateTime.AddDays(1);
                    schedulesUpdated = false;
                }
            }
            else
            {
                ImGui.Text("Please open the Supply & Demand window in the Workshop Agenda.");
            }
        }
        else
        {
            if (schedulesUpdated && schedules == null)
            {
                ImGui.Text("Tomorrow is a rest day.");
            }
            else if (schedules != null)
            {
                ImGui.Text($"Schedules for cycle {cycle}.");

                bool first = true;

                foreach (var schedule in schedules.OrderByDescending(x => x.Score).Take(3))
                {
                    DrawSchedule(schedule, first);

                    first = false;
                }
            }
            else
            {
                ImGui.Text($"Something went wrong, schedules == null = {schedules == null}");
            }
        }
    }

    private void DrawSchedule(Schedule schedule, bool first)
    {
        //ImGui.Text($"Score: {schedule.Score:0}");

        if (ImGui.CollapsingHeader($"Score: {schedule.Score:0}", first ? ImGuiTreeNodeFlags.None : ImGuiTreeNodeFlags.None))
        {
            if (ImGui.BeginTable("Schedule", 4))
            {
                ImGui.TableSetupColumn("#", ImGuiTableColumnFlags.None, 24);
                ImGui.TableSetupColumn("Handicraft", ImGuiTableColumnFlags.None, 180);
                ImGui.TableSetupColumn("Time", ImGuiTableColumnFlags.None, 40);
                ImGui.TableSetupColumn("Categories", ImGuiTableColumnFlags.None, 240);
                ImGui.TableHeadersRow();

                for (int i = 0; i < schedule.Handicrafts.Length; i++)
                {
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    TextRightAligned($"${i}.");

                    ImGui.TableSetColumnIndex(1);
                    ImGui.Text(schedule.Handicrafts[i].Name);

                    ImGui.TableSetColumnIndex(2);
                    TextRightAligned($"{schedule.Handicrafts[i].CraftingTime}h");

                    ImGui.TableSetColumnIndex(3);
                    ImGui.Text(string.Join(", ", schedule.Handicrafts[i].Categories.Select(x => x.Name)));
                }

                ImGui.EndTable();
            }

            var materials = schedule.Handicrafts
                .SelectMany(x => x.Materials)
                .GroupBy(x => x.InventoryItem)
                .Select(x => new { InventoryItem = x.Key, Amount = x.Sum(y => y.Amount) })
                .OrderBy(x => x.InventoryItem.Id);

            if (ImGui.BeginTable("Materials", 2))
            {
                ImGui.TableSetupColumn("Amount", ImGuiTableColumnFlags.None, 60);
                ImGui.TableSetupColumn("Material", ImGuiTableColumnFlags.None, 180);
                ImGui.TableHeadersRow();

                foreach (var material in materials)
                {
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    TextRightAligned((material.Amount * 3).ToString());

                    ImGui.TableSetColumnIndex(1);
                    ImGui.Text(material.InventoryItem.Name);
                }

                ImGui.EndTable();
            }
        }
    }

    private static void TextRightAligned(string text)
    {
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + ImGui.GetColumnWidth() - ImGui.CalcTextSize(text).X - ImGui.GetScrollX());
        ImGui.Text(text);

    }
}
