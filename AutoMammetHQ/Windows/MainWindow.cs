using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AutoMammetHQ.Data;
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

    public MainWindow(Plugin plugin) : base(
        "AutoMammetHQ", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize)
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
        //var inventoryItems = reader.InventoryItems;

        //int i = 0;

        //foreach (var inventoryItem in inventoryItems.Where(x => x.Category.Id == 4))
        //{
        //    bool apa = false;

        //    if (ImGui.Checkbox(inventoryItem.Name, ref apa))
        //    {
        //        var bepa = apa;
        //    }

        //    if (i++ % 4 != 3)
        //    {
        //        ImGui.SameLine();
        //    }
        //}

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        if (DateTime.UtcNow >= nextSupplyAndDemandUpdateTime)
        {
            ImGui.Text("Please open the supply and demand window in order to load the current supply and demand for export.");
            ImGui.Text($"Next time: {nextSupplyAndDemandUpdateTime}");
            ImGui.Text($"IsSupplyAndDemandAvailable: {reader.IsSupplyAndDemandAvailable()}");

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            if (reader.IsSupplyAndDemandAvailable())
            {
                nextSupplyAndDemandUpdateTime = DateTime.SpecifyKind(
                    new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 8, 0, 0), DateTimeKind.Utc);

                if (DateTime.UtcNow.TimeOfDay > new TimeSpan(8, 0, 0))
                {
                    nextSupplyAndDemandUpdateTime = nextSupplyAndDemandUpdateTime.AddDays(1);
                    schedulesUpdated = false;
                }
            }
        }
        else
        {
            if (ImGui.Button("Get schedule for tomorrow"))
            {
                var supplyAndDemand = reader.GetSupplyAndDemand();
                handicrafts = reader.Handicrafts;

                scheduleHandler = new ScheduleHandler(handicrafts, supplyAndDemand);
                schedules = scheduleHandler.GetSchedules();

                schedulesUpdated = true;
            }

            ImGui.SameLine();

            ImGui.Text("(This might take some time.)");

            if (schedulesUpdated && schedules == null)
            {
                ImGui.Text("Tomorrow is a rest day.");
            }
            else if (schedules != null)
            {
                var schedule = schedules.OrderByDescending(x => x.Score).First();

                DrawSchedule(schedule);

                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Spacing();
            }
        }
    }

    private void DrawSchedule(Schedule schedule)
    {
        ImGui.Text($"Score: {schedule.Score:0}");

        if (ImGui.BeginTable("Schedule", 4))
        {
            ImGui.TableSetupColumn("#");
            ImGui.TableSetupColumn("Handicraft");
            ImGui.TableSetupColumn("Time");
            ImGui.TableSetupColumn("Categories");
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
            ImGui.TableSetupColumn("Amount");
            ImGui.TableSetupColumn("Material");
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

    private static void TextRightAligned(string text)
    {
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + ImGui.GetColumnWidth() - ImGui.CalcTextSize(text).X - ImGui.GetScrollX());
        ImGui.Text(text);

    }
}
