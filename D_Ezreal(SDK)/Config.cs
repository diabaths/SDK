
using System.Linq;

using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.UI.IMenu;
using LeagueSharp.SDK.Core.UI.IMenu.Values;

using SharpDX;

namespace D_Ezreal_SDK_
{
    internal static class Config
    {
        private const string MenuName = "D-Ezreal";
        internal static object Drawings;
        internal static readonly Menu Menu;

        static Config()
        {
            Menu = new Menu(MenuName, MenuName, true).Attach();

            Keys.Initialize();
            Modes.Initialize();
            Modes.Items.Initialize();
            Modes.Misc.Initialize();
            Modes.Drawings.Initialize();
        }

        internal static void Initialize()
        {
        }

        internal static class Keys
        {
            internal static readonly Menu Menu;

            private static readonly MenuKeyBind _comboKey;

            private static readonly MenuKeyBind _harassKey;

            private static readonly MenuKeyBind _laneClearKey;

            private static readonly MenuKeyBind _jugnleClearKey;


            internal static bool ComboActive => _comboKey.Active;

            internal static bool HarassActive => _harassKey.Active;

            internal static bool LaneClearActive => _laneClearKey.Active;

            internal static bool JungleClearActive => _jugnleClearKey.Active;



            static Keys()
            {
                Menu = Config.Menu.Add(new Menu("Keys", "Keys"));

                _comboKey =
                    Menu.Add(new MenuKeyBind("ComboKey", "Combo", System.Windows.Forms.Keys.Space, KeyBindType.Press));
                _harassKey =
                    Menu.Add(new MenuKeyBind("HarassKey", "Harass", System.Windows.Forms.Keys.C, KeyBindType.Press));
                _laneClearKey =
                    Menu.Add(
                        new MenuKeyBind("LaneClearKey", "LaneClear", System.Windows.Forms.Keys.V, KeyBindType.Press));
                _jugnleClearKey =
                    Menu.Add(
                        new MenuKeyBind("JungleClearKey", "JungleClear", System.Windows.Forms.Keys.V, KeyBindType.Press));
            }

            internal static void Initialize()
            {
            }
        }

        internal static class Modes
        {
            internal static readonly Menu Menu;

            static Modes()
            {
                Menu = Config.Menu.Add(new Menu("Modes", "Modes"));

                Combo.Initialize();
                Harass.Initialize();
                LaneClear.Initialize();
                JungleClear.Initialize();
            }

            internal static void Initialize()
            {
            }

            internal static class Combo
            {
                internal static readonly Menu Menu;

                private static readonly MenuBool _useIgnitecombo;

                private static readonly MenuBool _useQ;

                private static readonly MenuBool _useW;

                private static readonly MenuBool _useR;

                private static readonly MenuBool _userr;

                private static readonly MenuSliderButton _Usermin;

                private static readonly MenuSliderButton _minrange;

                private static readonly MenuSliderButton _Ignitehealth;
                
                internal static bool UseIgnitecombo => _useIgnitecombo.Value;

                internal static bool UseQ => _useQ.Value;

                internal static bool UseW => _useW.Value;

                internal static bool UseR => _useR.Value;

                internal static bool Userr => _userr.Value;

                internal static int Minrange => _minrange.Value;

                internal static int Usermin => _Usermin.Value;

                internal static int Ignitehealth => _Ignitehealth.Value;

                static Combo()
                {
                    Menu = Modes.Menu.Add(new Menu("Combo", "Combo"));
                    _useIgnitecombo = Menu.Add(new MenuBool("UseIgnitecombo", "Use Ignite", true));
                    _Ignitehealth =
                        Menu.Add(
                            new MenuSliderButton("Ignitehelth", "Use Ignite if Enemy Hp %<", 30, 0, 100)
                                {
                                    BValue = true
                                });
                    _useQ = Menu.Add(new MenuBool("UseQ", "Use Q", true));
                    _useW = Menu.Add(new MenuBool("UseW", "Use W", true));
                    _useR = Menu.Add(new MenuBool("UseR", "Use R", true));
                    _minrange =
                        Menu.Add(new MenuSliderButton("Minrange", "Min Range to Use R", 700, 0, 1500) { BValue = true });
                    _userr = Menu.Add(new MenuBool("userr", "Use R if will hit x Targets", false));
                    _Usermin =
                       Menu.Add(new MenuSliderButton("Usermin", "Use R if will hit > ", 3, 1, 5) { BValue = true });
                }

                internal static void Initialize()
                {
                }
            }

            internal static class Harass
            {
                internal static readonly Menu Menu;

                private static readonly MenuBool _useQ;

                private static readonly MenuBool _useW;

                private static  MenuBool _useHa;

                private static readonly MenuSliderButton _minMana;

                internal static bool UseQ => _useQ.Value;

                internal static bool UseW => _useW.Value;

                internal static bool Useha => _useHa.Value;

                internal static int Mana => _minMana.Value;

                static Harass()
                {
                    Menu = Modes.Menu.Add(new Menu("Harass", "Harass"));

                    _useQ = Menu.Add(new MenuBool("UseQ", "Use Q", true));
                    _useW = Menu.Add(new MenuBool("UseW", "Use W", true));
                    if (GameObjects.EnemyHeroes.Any())
                    {
                        GameObjects.EnemyHeroes.ForEach(
                            i => _useHa = Menu.Add(new MenuBool("RCast" + i.ChampionName, "Cast On " + i.ChampionName, false)));
                    }
                    _minMana = Menu.Add(new MenuSliderButton("Mana", "Min Mana %", 70, 0, 100) { BValue = true });
                }

                internal static void Initialize()
                {
                }
            }

            internal static class LaneClear
            {
                internal static readonly Menu Menu;
                private static readonly MenuBool _useQ;
                private static readonly MenuBool _useW;
                private static readonly MenuSliderButton _minMana;

                internal static bool UseQ => _useQ.Value;
                internal static bool UseW => _useW.Value;
                internal static int MinMana => _minMana.Value;

                static LaneClear()
                {
                    Menu = Modes.Menu.Add(new Menu("LaneClear", "LaneClear"));

                    _useQ = Menu.Add(new MenuBool("UseQ", "Use Q", true));
                    _useW = Menu.Add(new MenuBool("UseW", "Use W(under enemy tower)", true));
                    _minMana = Menu.Add(new MenuSliderButton("MinMana", "Min Mana %", 70, 0, 100) { BValue = true });
                }

                internal static void Initialize()
                {
                }
            }

            internal static class JungleClear
            {
                internal static readonly Menu Menu;

                private static readonly MenuBool _useQ;

                private static readonly MenuSliderButton _minMana;

                internal static bool UseQ => _useQ.Value;

                internal static int MinMana => _minMana.Value;

                static JungleClear()
                {
                    Menu = Modes.Menu.Add(new Menu("JungleClear", "JungleClear"));

                    _useQ = Menu.Add(new MenuBool("UseQ", "Use Q", true));
                    _minMana = Menu.Add(new MenuSliderButton("MinMana", "Min Mana %", 0, 0, 100) { BValue = true });
                }

                internal static void Initialize()
                {
                }
            }



            internal static class Items
            {
                internal static readonly Menu Menu;

                static Items()
                {
                    Menu = Config.Menu.Add(new Menu("Items", "items"));

                    Potions.Initialize();
                    Offensive.Initialize();
                    Deffensive.Initialize();
                }

                internal static void Initialize()
                {
                }

                internal static class Potions
                {
                    internal static readonly Menu Menu;

                    private static readonly MenuBool _usehppotions;

                    private static readonly MenuSliderButton _hpmin;

                    private static readonly MenuBool _usemppotions;

                    private static readonly MenuSliderButton _mpmin;



                    internal static bool UseHPpotion => _usehppotions.Value;

                    internal static int minHP => _hpmin.Value;

                    internal static bool UseMPpotion => _usemppotions.Value;

                    internal static int MinMP => _mpmin.Value;

                    static Potions()
                    {
                        Menu = Items.Menu.Add(new Menu("Potions", "Potions"));

                        _usehppotions =
                            Menu.Add(
                                new MenuBool(
                                    "UseHPpotion",
                                    "Use Healt potion/Refillable/Hunters/Corrupting/Biscuit",
                                    true));
                        _hpmin = Menu.Add(new MenuSliderButton("minHP", "If Health % <", 50, 0, 100));
                        _usemppotions = Menu.Add(new MenuBool("UseMPpotion", "Use Hunters/Corrupting/Biscuit", true));
                        _mpmin = Menu.Add(new MenuSliderButton("MinMP", "If Mana % <", 50, 0, 100));
                      
                    }

                    internal static void Initialize()
                    {
                    }
                }

                internal static class Offensive
                {

                    internal static readonly Menu Menu;

                    private static readonly MenuBool _Youmuu;

                    private static readonly MenuBool _Blade;

                    private static readonly MenuSliderButton _BladeEnemyhp;

                    private static readonly MenuSliderButton _Blademyhp;

                    private static readonly MenuBool _Hextech;

                    private static readonly MenuSliderButton _HextechEnemyhp;

                    internal static bool Youmuu => _Youmuu.Value;

                    internal static bool Blade => _Blade.Value;

                    internal static int BladeEnemyhp => _BladeEnemyhp.SValue;

                    internal static int Blademyhp => _Blademyhp.SValue;

                    internal static bool Hextech => _Hextech.Value;

                    internal static int HextechEnemyhp => _HextechEnemyhp.SValue;

                    static Offensive()
                    {
                        Menu = Items.Menu.Add(new Menu("Offensive", "Offensive"));

                        _Youmuu = Menu.Add(new MenuBool("Youmuu", "Use Youmuu's", true));
                        _Blade = Menu.Add(new MenuBool("Blade", "Use Bilge/Blade", true));
                        _BladeEnemyhp =
                            Menu.Add(
                                new MenuSliderButton("BladeEnemyhp", "If Enemy Hp <", 70, 0, 100, true)
                                    {
                                        BValue = true
                                    });
                        _Blademyhp =
                            Menu.Add(
                                new MenuSliderButton("Blademyhp", "If My Hp <", 70, 0, 100, true) { BValue = true });
                        _Hextech = Menu.Add(new MenuBool("Hextech", "Hextech Gunblade", true));
                        _HextechEnemyhp =
                            Menu.Add(
                                new MenuSliderButton("HextechEnemyhp", "If Enemy Hp <", 70, 0, 100, true)
                                    {
                                        BValue =
                                            true
                                    });
                    }

                    internal static void Initialize()
                    {
                    }
                }

                internal static class Deffensive
                {
                    internal static readonly Menu Menu;

                    private static readonly MenuSliderButton _Archangelmyhp;

                    private static readonly MenuBool _Archangel;

                    internal static bool Archangel => _Archangel.Value;

                    internal static int Archangelmyhp => _Archangelmyhp.Value;

                    static Deffensive()
                    {
                        Menu = Items.Menu.Add(new Menu("Deffensive", "Deffensive"));

                        _Archangel = Menu.Add(new MenuBool("Archangel", " Use Seraph's Embrace", true));
                        _Archangelmyhp =
                            Menu.Add(
                                new MenuSliderButton("Archangelmyhp", "If My Hp <", 70, 0, 100, true) { BValue = true });
                    }

                    internal static void Initialize()
                    {
                    }
                }
            }


            internal static class Misc
            {
                internal static readonly Menu Menu;
               
                private static readonly MenuBool _useQK;

                private static readonly MenuBool _useWK;

                private static readonly MenuBool _UseRM;

                private static readonly MenuBool _useQdash;

                private static readonly MenuBool _useQimmo;

                private static readonly MenuBool _Gap_E;
                
                internal static bool useQK => _useQK.Value;

                internal static bool useWK => _useWK.Value;

                internal static bool UseRM => _UseRM.Value;

                internal static bool useQdash => _useQdash.Value;

                internal static bool useQimmo => _useQimmo.Value;
                
                internal static bool Gap_E => _Gap_E.Value;




                static Misc()
                {
                    Menu = Config.Menu.Add(new Menu("Misc", "Misc"));

                    _useQK = Menu.Add(new MenuBool("useQK", "Use Q to Killsteal", true));
                    _useWK = Menu.Add(new MenuBool("useWK", "Use W to Killstea", true));
                    _UseRM = Menu.Add(new MenuBool("UseRM", "Use R to Killstea", true));
                    _useQdash = Menu.Add(new MenuBool("useQdash", "Auto Q dashing", true));
                    _useQimmo = Menu.Add(new MenuBool("useQimmo", "Auto Q Immobile", true));
                     _Gap_E = Menu.Add(new MenuBool("Gap_E", "Use E to Gapcloser", true));
                }

                internal static void Initialize()
                {
                }
            }

            internal static class Drawings
            {
                internal static readonly Menu Menu;

                private static readonly MenuBool _HerosEnabled;

                private static readonly MenuBool _drawQRange;

                private static readonly MenuBool _drawWRange;

                private static readonly MenuBool _drawERange;

                private static readonly MenuBool _drawRRange;

                private static readonly MenuBool _DrawQDamage;

                private static readonly MenuBool _DrawWDamage;

                private static readonly MenuBool _DrawRDamage;

                private static readonly MenuColor _QDamageColor;

                private static readonly MenuColor _WDamageColor;

                private static readonly MenuColor _RDamageColor;

                private static readonly MenuBool _DamageIndicatorEnabled;

                internal static bool HerosEnabled => _HerosEnabled.Value;

                internal static bool DrawQRange => _drawQRange.Value;

                internal static bool DrawWRange => _drawWRange.Value;

                internal static bool DrawERange => _drawERange.Value;

                internal static bool DrawRRange => _drawRRange.Value;

                internal static bool DamageIndicatorEnabled => _DamageIndicatorEnabled.Value;

                internal static bool DrawQDamage => _DrawQDamage.Value;

                internal static bool DrawWDamage => _DrawWDamage.Value;

                internal static bool DrawRDamage => _DrawRDamage.Value;

                internal static Color QDamageColor => _QDamageColor.Color;

                internal static Color WDamageColor => _WDamageColor.Color;

                internal static Color RDamageColor => _RDamageColor.Color;


                static Drawings()
                {
                    Menu = Config.Menu.Add(new Menu("Drawings", "Drawings"));

                    Menu.Add(new MenuSeparator("SpellRange", "Spell Range"));
                    _drawQRange = Menu.Add(new MenuBool("drawQRange", "Draw Q Range"));
                    _drawWRange = Menu.Add(new MenuBool("drawWRange", "Draw W Range"));
                    _drawERange = Menu.Add(new MenuBool("drawERange", "Draw E Range"));
                    _drawRRange = Menu.Add(new MenuBool("drawRRange", "Draw R Range"));
                    _DrawQDamage = Menu.Add(new MenuBool("DrawQDamage", "Draw Q Damage", true));
                    _DrawWDamage = Menu.Add(new MenuBool("DrawWDamage", "Draw W Damage", true));
                    _DrawRDamage = Menu.Add(new MenuBool("DrawRDamage", "Draw R Damage", true));
                    _QDamageColor = Menu.Add(new MenuColor("QdamageColor", "Q Damage Color", Color.Orange));
                    _WDamageColor = Menu.Add(new MenuColor("WdamageColor", "W Damage Color", Color.MediumSpringGreen));
                    _RDamageColor = Menu.Add(new MenuColor("RdamageColor", "R Damage Color", Color.MediumSpringGreen));
                    _HerosEnabled = Menu.Add(new MenuBool("HerosEnabled", "Draw on Heros", true));


                    Menu.Add(new MenuSeparator("DamageIndicator", "Damage Indicator"));
                    _DamageIndicatorEnabled =
                        Menu.Add(new MenuBool("DamageIndicatorEnabled", "DamageIndicator Enabled", true));

                }

                internal static void Initialize()
                {
                }
            }
        }
    }
}
