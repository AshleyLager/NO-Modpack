// ============================================================
// Fuel Burn Endurance Timer
// GUID: Com.Hellcat92.FuelBurnEnduranceTimer_3.0.0
// Made by Hellcat92
// Version: 3.0.0
// Date: 31 July 2026
// ============================================================

using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using TMPro;

namespace FuelBurnHUD
{
    public enum RangeUnit
    {
        Km,
        Nm,
        Mile
    }

    [BepInPlugin("com.hellcat92.fuelburnhud", "Fuel Burn Endurance Timer", "3.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> ModEnabled;

        public static ConfigEntry<bool> ShowFUEL;
        public static ConfigEntry<bool> ShowTIMEREM;
        public static ConfigEntry<bool> ShowFLOW;
        public static ConfigEntry<bool> ShowRNG;

        public static ConfigEntry<int> FontSize;

        public static ConfigEntry<Color> HudColor;

        // TRUE = Metric (KG), FALSE = Imperial (LB)
        public static ConfigEntry<bool> UseMetricUnits;

        // Dropdown: Km, Nm, Mile
        public static ConfigEntry<RangeUnit> RangeUnits;

        // Locked offsets
        public const int LockedHorizontalOffset = -310;
        public const int LockedVerticalOffset = -120;

        private void Awake()
        {
            Logger.LogInfo("Fuel Burn Endurance Timer 3.0.0 Loaded");

            ModEnabled = Config.Bind("General", "Enable Mod", true);

            ShowFUEL = Config.Bind("HUD", "Show FUEL", true);
            ShowTIMEREM = Config.Bind("HUD", "Show TIME REM", true);
            ShowFLOW = Config.Bind("HUD", "Show FLOW", true);
            ShowRNG = Config.Bind("HUD", "Show RNG", true);

            FontSize = Config.Bind("HUD", "Font Size", 16);

            HudColor = Config.Bind("HUD", "HUD Text Color", new Color(0.3f, 1f, 0.3f, 1f));

            // Toggle: Metric (KG) / Imperial (LB)
            UseMetricUnits = Config.Bind(
                "HUD",
                "Use Metric Units",
                false,
                "Toggle between Imperial (off) and Metric (on)"
            );

            // Dropdown: Km, Nm, Mile
            RangeUnits = Config.Bind(
                "HUD",
                "Range Units",
                RangeUnit.Km,
                "Choose Km, Nm, or Mile for range display"
            );

            gameObject.AddComponent<FuelBurnWatcher>();
        }

        public static Color32 GetHudColor()
        {
            Color c = HudColor.Value;
            return new Color32(
                (byte)(c.r * 255),
                (byte)(c.g * 255),
                (byte)(c.b * 255),
                (byte)(c.a * 255)
            );
        }
    }

    public class FuelBurnWatcher : MonoBehaviour
    {
        private CombatHUD hud;
        private Aircraft aircraft;
        private Transform hudCenter;

        private FuelBurnController controller;

        private void Update()
        {
            var newHud = FindObjectOfType<CombatHUD>();
            if (newHud != hud)
            {
                hud = newHud;
                ResetInjection();
            }

            if (hud == null)
                return;

            if (aircraft != hud.aircraft)
            {
                aircraft = hud.aircraft;
                ResetInjection();
            }

            if (aircraft == null)
                return;

            var fh = SceneSingleton<FlightHud>.i;
            if (fh == null)
                return;

            var newCenter = fh.GetHUDCenter();
            if (newCenter != hudCenter)
            {
                hudCenter = newCenter;
                ResetInjection();
            }

            if (hudCenter == null)
                return;

            if (controller == null)
                InjectHUD();
        }

        private void ResetInjection()
        {
            if (controller != null)
            {
                Destroy(controller.gameObject);
                controller = null;
            }
        }

        private void InjectHUD()
        {
            GameObject go = new GameObject("FuelBurnHUD");
            go.transform.SetParent(hudCenter, false);

            controller = go.AddComponent<FuelBurnController>();
            controller.aircraft = aircraft;
        }
    }

    public class FuelBurnController : MonoBehaviour
    {
        public Aircraft aircraft;

        private TextMeshProUGUI fuelText;
        private TextMeshProUGUI timeRemText;
        private TextMeshProUGUI flowText;
        private TextMeshProUGUI rangeText;

        private float lastFuelKg;
        private float lastTime;
        private float flowKgPerSec;
        private bool hasSample;

        private const float KgToLb = 2.20462262f;

        private void Start()
        {
            fuelText = CreateTMP("FUEL");
            timeRemText = CreateTMP("TIME REM");
            flowText = CreateTMP("FLOW");
            rangeText = CreateTMP("RNG");
        }

        private TextMeshProUGUI CreateTMP(string name)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(this.transform, false);

            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.color = Plugin.GetHudColor();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = false;

            RectTransform rt = tmp.rectTransform;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(800f, 40f);

            return tmp;
        }

        private void LateUpdate()
        {
            if (!Plugin.ModEnabled.Value)
            {
                fuelText.text = "";
                timeRemText.text = "";
                flowText.text = "";
                rangeText.text = "";
                return;
            }

            if (aircraft == null)
                return;

            var tanks = aircraft.GetFuelTanks();
            if (tanks == null || tanks.Count == 0)
                return;

            float totalKg = 0f;
            foreach (var t in tanks)
                if (t != null) totalKg += t.fuelMass;

            float now = Time.time;

            if (lastTime == 0f)
            {
                lastTime = now;
                lastFuelKg = totalKg;
                flowKgPerSec = 0f;
                hasSample = false;
            }

            if (now - lastTime >= 1f)
            {
                float delta = lastFuelKg - totalKg;
                float dt = now - lastTime;

                lastTime = now;
                lastFuelKg = totalKg;

                if (delta > 0.01f && dt > 0f)
                {
                    flowKgPerSec = delta / dt;
                    hasSample = true;
                }
                else
                {
                    flowKgPerSec = 0f;
                    hasSample = false;
                }
            }

            float enduranceSec =
                (hasSample && flowKgPerSec > 0f && totalKg > 0f)
                ? totalKg / flowKgPerSec
                : 0f;

            int baseX = Plugin.LockedHorizontalOffset;
            int baseY = Plugin.LockedVerticalOffset;

            fuelText.fontSize = Plugin.FontSize.Value;
            timeRemText.fontSize = Plugin.FontSize.Value;
            flowText.fontSize = Plugin.FontSize.Value;
            rangeText.fontSize = Plugin.FontSize.Value;

            fuelText.color = Plugin.GetHudColor();
            timeRemText.color = Plugin.GetHudColor();
            flowText.color = Plugin.GetHudColor();
            rangeText.color = Plugin.GetHudColor();

            fuelText.rectTransform.anchoredPosition = new Vector2(baseX, baseY);
            timeRemText.rectTransform.anchoredPosition = new Vector2(baseX, baseY - 20);
            flowText.rectTransform.anchoredPosition = new Vector2(baseX, baseY - 40);
            rangeText.rectTransform.anchoredPosition = new Vector2(baseX, baseY - 60);

            bool metric = Plugin.UseMetricUnits.Value;

            // FUEL
            if (Plugin.ShowFUEL.Value)
            {
                if (metric)
                    fuelText.text = $"FUEL [{totalKg:0}] kg";
                else
                    fuelText.text = $"FUEL [{totalKg * KgToLb:0}] lb";
            }
            else fuelText.text = "";

            // TIME REM
            if (Plugin.ShowTIMEREM.Value)
            {
                if (!hasSample || enduranceSec <= 0f)
                {
                    timeRemText.text = "TIME REM (--:--:--)";
                }
                else
                {
                    int h = Mathf.FloorToInt(enduranceSec / 3600);
                    int m = Mathf.FloorToInt((enduranceSec % 3600) / 60);
                    int s = Mathf.FloorToInt(enduranceSec % 60);
                    timeRemText.text = $"TIME REM ({h}:{m:D2}:{s:D2})";
                }
            }
            else timeRemText.text = "";

            // FLOW
            if (Plugin.ShowFLOW.Value)
            {
                if (!hasSample || flowKgPerSec <= 0f)
                {
                    flowText.text = metric ? "FLOW [--] kg/s" : "FLOW [--] lb/s";
                }
                else
                {
                    if (metric)
                        flowText.text = $"FLOW [{flowKgPerSec:0.0}] kg/s";
                    else
                        flowText.text = $"FLOW [{flowKgPerSec * KgToLb:0.0}] lb/s";
                }
            }
            else flowText.text = "";

            // RANGE
            if (Plugin.ShowRNG.Value)
            {
                if (!hasSample || enduranceSec <= 0f || aircraft.rb == null)
                {
                    rangeText.text = "RNG ----";
                }
                else
                {
                    float gs = aircraft.rb.velocity.magnitude;
                    float meters = gs * enduranceSec;

                    switch (Plugin.RangeUnits.Value)
                    {
                        case RangeUnit.Km:
                            float km = meters / 1000f;
                            rangeText.text = $"RNG {km:0}km";
                            break;

                        case RangeUnit.Mile:
                            float mi = meters / 1609.34f;
                            rangeText.text = $"RNG {mi:0}mi";
                            break;

                        default: // Nm
                            float nm = meters / 1852f;
                            rangeText.text = $"RNG {nm:0}nm";
                            break;
                    }
                }
            }
            else rangeText.text = "";
        }
    }
}
