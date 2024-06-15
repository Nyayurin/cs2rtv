﻿using System.Text.RegularExpressions;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using Microsoft.Extensions.Logging;
using Timer = CounterStrikeSharp.API.Modules.Timers.Timer;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime.InteropServices;
namespace cs2rtv;

public class Cs2rtv : BasePlugin
{
    public override string ModuleAuthor => "lapl";
    public override string ModuleName => "MapChanger for kz/bhop/surf";
    public override string ModuleVersion => "1.0.0";
    private List<string> maplist = new();
    private List<string> mapnominatelist = new();
    private List<ulong> rtvcount = new();
    private List<ulong> extcount = new();
    private List<string> votemaplist = new();
    private List<string> mapcooldown = new();
    private bool canrtv = false;
    private bool firstmaprandom = false;
    private bool isrtving = false;
    private bool isrtv = false;
    private bool rtvwin = false;
    private bool isrtvagain = false;
    private int playercount = 0;
    private int rtvrequired = 0;
    private int timeleft = 0;
    private int extround = 0;
    private Timer? _canrtvtimer;
    private Timer? _maptimer;
    private Timer? _rtvtimer;
    private Timer? _endmaptimer;
    private Timer? _changemaprepeat;
    private Timer? _repeattimer;
    private Random random = new();
    private List<string> rtvmusiclist =
    [
        "Music.BombPlanted.3kliksphilip_01",
        "Music.BombPlanted.amontobin_01",
        "Music.BombPlanted.austinwintory_01",
        "Music.BombPlanted.austinwintory_02",
        "Music.BombPlanted.austinwintory_03",
        "Music.BombPlanted.awolnation_01",
        "Music.BombPlanted.bbnos_01",
        "Music.BombPlanted.beartooth_01",
        "Music.BombPlanted.beartooth_02",
        "Music.BombPlanted.blitzkids_01",
        "Music.BombPlanted.chipzel_01",
        "Music.BombPlanted.damjanmravunac_01",
        "Music.BombPlanted.danielsadowski_01",
        "Music.BombPlanted.danielsadowski_02",
        "Music.BombPlanted.danielsadowski_03",
        "Music.BombPlanted.danielsadowski_04",
        "Music.BombPlanted.darude_01",
        "Music.BombPlanted.denzelcurry_01",
        "Music.BombPlanted.dren_01",
        "Music.BombPlanted.dren_02",
        "Music.BombPlanted.dryden_01",
        "Music.BombPlanted.feedme_01",
        "Music.BombPlanted.freakydna_01",
        "Music.BombPlanted.hades_01",
        "Music.BombPlanted.halflife_alyx_01",
        "Music.BombPlanted.halo_01",
        "Music.BombPlanted.hlb_01",
        "Music.BombPlanted.hotlinemiami_01",
        "Music.BombPlanted.hundredth_01",
        "Music.BombPlanted.ianhultquist_01",
        "Music.BombPlanted.isoxo_01",
        "Music.BombPlanted.jesseharlin_01",
        "Music.BombPlanted.juelz_01",
        "Music.BombPlanted.kellybailey_01",
        "Music.BombPlanted.killscript_01",
        "Music.BombPlanted.kitheory_01",
        "Music.BombPlanted.knock2_01",
        "Music.BombPlanted.knock2_02",
        "Music.BombPlanted.laurashigihara_01",
        "Music.BombPlanted.lenniemoore_01",
        "Music.BombPlanted.mateomessina_01",
        "Music.BombPlanted.mattlange_01",
        "Music.BombPlanted.mattlevine_01",
        "Music.BombPlanted.meechydarko_01",
        "Music.BombPlanted.michaelbross_01",
        "Music.BombPlanted.midnightriders_01",
        "Music.BombPlanted.mordfustang_01",
        "Music.BombPlanted.neckdeep_01",
        "Music.BombPlanted.neckdeep_02",
        "Music.BombPlanted.newbeatfund_01",
        "Music.BombPlanted.noisia_01",
        "Music.BombPlanted.perfectworld_01",
        "Music.BombPlanted.proxy_01",
        "Music.BombPlanted.radcat_01",
        "Music.BombPlanted.roam_01",
        "Music.BombPlanted.robertallaire_01",
        "Music.BombPlanted.sammarshall_01",
        "Music.BombPlanted.sarahschachner_01",
        "Music.BombPlanted.sasha_01",
        "Music.BombPlanted.scarlxrd_01",
        "Music.BombPlanted.scarlxrd_02",
        "Music.BombPlanted.seanmurray_01",
        "Music.BombPlanted.skog_01",
        "Music.BombPlanted.skog_02",
        "Music.BombPlanted.skog_03",
        "Music.BombPlanted.sullivanking_01",
        "Music.BombPlanted.theverkkars_01",
        "Music.BombPlanted.theverkkars_02",
        "Music.BombPlanted.timhuling_01",
        "Music.BombPlanted.treeadams_benbromfield_01",
        "Music.BombPlanted.troelsfolmann_01",
        "Music.BombPlanted.twerl_01",
        "Music.BombPlanted.twinatlantic_01",
        "Music.BombPlanted.valve_cs2_01",
        "Music.BombPlanted.valve_csgo_01",
        "Music.BombPlanted.valve_csgo_02"
    ];

    private List<string> mapendmusiclist =
    [
        "Music.BombTenSecCount.3kliksphilip_01",
        "Music.BombTenSecCount.amontobin_01",
        "Music.BombTenSecCount.austinwintory_01",
        "Music.BombTenSecCount.austinwintory_02",
        "Music.BombTenSecCount.austinwintory_03",
        "Music.BombTenSecCount.awolnation_01",
        "Music.BombTenSecCount.bbnos_01",
        "Music.BombTenSecCount.beartooth_01",
        "Music.BombTenSecCount.beartooth_02",
        "Music.BombTenSecCount.blitzkids_01",
        "Music.BombTenSecCount.chipzel_01",
        "Music.BombTenSecCount.damjanmravunac_01",
        "Music.BombTenSecCount.danielsadowski_01",
        "Music.BombTenSecCount.danielsadowski_02",
        "Music.BombTenSecCount.danielsadowski_03",
        "Music.BombTenSecCount.danielsadowski_04",
        "Music.BombTenSecCount.darude_01",
        "Music.BombTenSecCount.denzelcurry_01",
        "Music.BombTenSecCount.dren_01",
        "Music.BombTenSecCount.dren_02",
        "Music.BombTenSecCount.dryden_01",
        "Music.BombTenSecCount.feedme_01",
        "Music.BombTenSecCount.freakydna_01",
        "Music.BombTenSecCount.hades_01",
        "Music.BombTenSecCount.halflife_alyx_01",
        "Music.BombTenSecCount.halo_01",
        "Music.BombTenSecCount.hlb_01",
        "Music.BombTenSecCount.hotlinemiami_01",
        "Music.BombTenSecCount.hundredth_01",
        "Music.BombTenSecCount.ianhultquist_01",
        "Music.BombTenSecCount.isoxo_01",
        "Music.BombTenSecCount.jesseharlin_01",
        "Music.BombTenSecCount.juelz_01",
        "Music.BombTenSecCount.kellybailey_01",
        "Music.BombTenSecCount.killscript_01",
        "Music.BombTenSecCount.kitheory_01",
        "Music.BombTenSecCount.knock2_01",
        "Music.BombTenSecCount.knock2_02",
        "Music.BombTenSecCount.laurashigihara_01",
        "Music.BombTenSecCount.lenniemoore_01",
        "Music.BombTenSecCount.mateomessina_01",
        "Music.BombTenSecCount.mattlange_01",
        "Music.BombTenSecCount.mattlevine_01",
        "Music.BombTenSecCount.meechydarko_01",
        "Music.BombTenSecCount.michaelbross_01",
        "Music.BombTenSecCount.midnightriders_01",
        "Music.BombTenSecCount.mordfustang_01",
        "Music.BombTenSecCount.neckdeep_01",
        "Music.BombTenSecCount.neckdeep_02",
        "Music.BombTenSecCount.newbeatfund_01",
        "Music.BombTenSecCount.noisia_01",
        "Music.BombTenSecCount.perfectworld_01",
        "Music.BombTenSecCount.proxy_01",
        "Music.BombTenSecCount.radcat_01",
        "Music.BombTenSecCount.roam_01",
        "Music.BombTenSecCount.robertallaire_01",
        "Music.BombTenSecCount.sammarshall_01",
        "Music.BombTenSecCount.sarahschachner_01",
        "Music.BombTenSecCount.sasha_01",
        "Music.BombTenSecCount.scarlxrd_01",
        "Music.BombTenSecCount.scarlxrd_02",
        "Music.BombTenSecCount.seanmurray_01",
        "Music.BombTenSecCount.skog_01",
        "Music.BombTenSecCount.skog_02",
        "Music.BombTenSecCount.skog_03",
        "Music.BombTenSecCount.sullivanking_01",
        "Music.BombTenSecCount.theverkkars_01",
        "Music.BombTenSecCount.theverkkars_02",
        "Music.BombTenSecCount.timhuling_01",
        "Music.BombTenSecCount.treeadams_benbromfield_01",
        "Music.BombTenSecCount.troelsfolmann_01",
        "Music.BombTenSecCount.twerl_01",
        "Music.BombTenSecCount.twinatlantic_01",
        "Music.BombTenSecCount.valve_cs2_01",
        "Music.BombTenSecCount.valve_csgo_01",
        "Music.BombTenSecCount.valve_csgo_02"
    ];


    public override void Load(bool hotReload)
    {
        Logger.LogInformation("load maplist from {Path}", Path.Join(ModuleDirectory, "maplist.txt"));
        maplist = new List<string>(File.ReadAllLines(Path.Join(ModuleDirectory, "maplist.txt")));
        EmitSoundExtension.Init();

        if (hotReload)
        {
            Server.NextFrame(() =>
            {
                mapcooldown.Clear();
                mapcooldown.Add(Server.MapName);
                rtvwin = false;
                rtvcount.Clear();
                extcount.Clear();
                mapnominatelist.Clear();
                votemaplist.Clear();
                isrtv = false;
                isrtving = false;
                isrtvagain = false;
                canrtv = true;
                firstmaprandom = true;
                KillTimer();
                timeleft = 15;
                StartMaptimer();
            });
        }
        RegisterEventHandler<EventPlayerDisconnect>((@event, info) =>
        {
            if (rtvcount.Contains(@event.Userid!.SteamID))
                rtvcount.Remove(@event.Userid.SteamID);
            if (extcount.Contains(@event.Userid!.SteamID))
                extcount.Remove(@event.Userid.SteamID);
            GetPlayersCount();
            if (rtvcount.Count >= rtvrequired && playercount != 0)
            {
                isrtving = true;
                isrtv = true;
                rtvcount.Clear();
                var x = 0;
                _repeattimer = AddTimer(1f, () =>
                {
                    x++;
                    foreach (var player in IsPlayer())
                    {
                        playClientSound(player, "Alert.WarmupTimeoutBeep");
                        player.PrintToChat("地图投票即将开始");
                    }
                    if(x == 10)
                        Server.NextFrame(()=>StartRtv());
                }, TimerFlags.REPEAT);
            }
            if (extcount.Count >= rtvrequired && playercount != 0)
            {
                Server.PrintToChatAll("地图已延长");
                timeleft += 15;
                extcount.Clear();
                CanRtvtimer();
            }
            return HookResult.Continue;
        });

        RegisterListener<Listeners.OnMapStart>(OnMapStart =>
        {
            if (!hotReload && !firstmaprandom)
            {
                if (!Regex.IsMatch(Server.MapName, @$"\bde_"))
                {
                    Server.NextFrame(() =>
                    {
                        firstmaprandom = true;
                        Random random = new();
                        int index = random.Next(0, maplist.Count - 1);
                        var randommap = maplist[index];
                        if (randommap == Server.MapName)
                            return;
                        Server.ExecuteCommand($"ds_workshop_changelevel {randommap}");
                    });
                    return;
                }
            }
            Server.NextFrame(() =>
            {
                mapcooldown.Add(Server.MapName);
                if (mapcooldown.Count > 5)
                    mapcooldown.Remove(mapcooldown.First());
                rtvwin = false;
                rtvcount.Clear();
                extcount.Clear();
                mapnominatelist.Clear();
                votemaplist.Clear();
                isrtv = false;
                isrtving = false;
                isrtvagain = false;
                KillTimer();
                timeleft = 15;
                extround = 0;
                CanRtvtimer();
                StartMaptimer();
            });
        });
    }
    [ConsoleCommand("css_timeleft")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void TimeleftCommand(CCSPlayerController? cCSPlayer, CommandInfo command)
    {
        if(cCSPlayer != null)
            cCSPlayer.PrintToChat($"当前地图还剩余 {timeleft} 分钟");
        else
            Server.PrintToConsole($"当前地图还剩余 {timeleft} 分钟");
    }

    [ConsoleCommand("css_stopsound")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void StopSoundCommand(CCSPlayerController? cCSPlayer, CommandInfo command)
    {
        playClientSound(cCSPlayer!,"StopSoundEvents.StopAllMusic");
    }

    [ConsoleCommand("css_rtv")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void RtvCommand(CCSPlayerController? cCSPlayer, CommandInfo command)
    {
        if (!canrtv)
        {
            command.ReplyToCommand("投票冷却中。。。");
            return;
        }
        if (isrtving)
        {
            command.ReplyToCommand("投票已在进行中");
            return;
        }
        GetPlayersCount();

        if (rtvcount.Contains(cCSPlayer!.SteamID))
        {
            Server.PrintToChatAll($"{cCSPlayer.PlayerName} 已投票更换地图，当前 {rtvcount.Count}/{rtvrequired}");
            return;
        }
        rtvcount.Add(cCSPlayer.SteamID);
        if (rtvcount.Count < rtvrequired)
        {
            Server.PrintToChatAll($"{cCSPlayer.PlayerName} 已投票更换地图，当前 {rtvcount.Count}/{rtvrequired}");
        }
        else
        {
            isrtving = true;
            isrtv = true;
            rtvcount.Clear();
            var x = 0;
            _repeattimer = AddTimer(1f, () =>
            {
                x++;
                foreach (var player in IsPlayer())
                {
                    playClientSound(player, "Alert.WarmupTimeoutBeep");
                    player.PrintToChat("地图投票即将开始");
                }
                if(x == 10)
                    Server.NextFrame(()=>StartRtv());
            }, TimerFlags.REPEAT);
        }
    }

    [ConsoleCommand("css_forceext")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    [RequiresPermissions("@css/changemap")]
    public void ForceExtCommand(CCSPlayerController? cCSPlayer,CommandInfo command)
    {
        timeleft+=15;
        Server.PrintToChatAll("管理员已延长地图");
    }

    [ConsoleCommand("css_ext")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void ExtCommand(CCSPlayerController? cCSPlayer, CommandInfo command)
    {
        if (!canrtv)
        {
            command.ReplyToCommand("投票冷却中。。。");
            return;
        }
        if (isrtving)
        {
            command.ReplyToCommand("投票已在进行中");
            return;
        }
        if(extround >= 3)
        {
            command.ReplyToCommand("已达到延长命令上限，请在下次正常投票过程中决定是否延长");
            return;
        }
        GetPlayersCount();
        if (extcount.Contains(cCSPlayer!.SteamID))
        {
            Server.PrintToChatAll($"{cCSPlayer.PlayerName} 已投票延长地图，当前 {extcount.Count}/{rtvrequired}");
            return;
        }
        extcount.Add(cCSPlayer.SteamID);
        if (extcount.Count < rtvrequired)
        {
            Server.PrintToChatAll($"{cCSPlayer.PlayerName} 已投票延长地图，当前 {extcount.Count}/{rtvrequired}");
        }
        else
        {
            CanRtvtimer();
            Server.PrintToChatAll("地图已延长");
            timeleft += 15;
            extround++;
            extcount.Clear();
        }
    }

    [ConsoleCommand("css_forcertv")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    [RequiresPermissions("@css/changemap")]
    public void ForceRtvCommand(CCSPlayerController? cCSPlayer, CommandInfo command)
    {
        if (isrtving)
        {
            command.ReplyToCommand("投票已在进行中");
            return;
        }
        isrtving = true;
        isrtv = true;
        var x = 0;
        _repeattimer = AddTimer(1f, () =>
        {
            x++;
            foreach (var player in IsPlayer())
            {
                playClientSound(player, "Alert.WarmupTimeoutBeep");
                player.PrintToChat("管理员已强制开始地图投票");
            }
            if(x == 10)
                Server.NextFrame(()=>StartRtv());
        }, TimerFlags.REPEAT);
    }

    [ConsoleCommand("css_nominate")]
    [CommandHelper(minArgs: 0, usage: "[mapname]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void NominateCommand(CCSPlayerController? cCSPlayer, CommandInfo command)
    {
        if (isrtving)
        {
            command.ReplyToCommand("投票已在进行中");
            return;
        }
        if (mapnominatelist.Count >= 5)
        {
            command.ReplyToCommand("当前预定地图已满");
            return;
        }
        string? mapname = command.GetArg(1);

        List<string> blocklist = new List<string>
        {
            "surf","surf_","bhop","bhop_","kz","kz_"
        };
        foreach (string bug in blocklist)
        {
            if (Regex.IsMatch(mapname, @$"\b{bug}\b"))
            {
                command.ReplyToCommand($"你输入的字段太少，无法查到符合条件的地图");
                return;
            }
        }

        if (maplist.Contains(mapname) && mapname.Length > 2)
        {
            mapname = maplist.Find(x => Regex.IsMatch(mapname, x));
            if(mapname == null)
            {
                List<string> findmapcache = maplist.Where(x => x.Contains(mapname!)).ToList();
                var randommap = findmapcache.FirstOrDefault();
                command.ReplyToCommand($"你是否在寻找 {randommap}");
                return;
            }else
            {
                if (mapnominatelist.Find(x => Regex.IsMatch(mapname, x)) != null)
                {
                    command.ReplyToCommand($"地图 {mapname} 已被他人预定");
                    return;
                }
                else if (mapname == Server.MapName)
                {
                    command.ReplyToCommand($"地图 {mapname} 为当前地图");
                    return;
                }
                else if (mapcooldown.Find(x => Regex.IsMatch(mapname, x)) != null)
                {
                    command.ReplyToCommand($"地图 {mapname} 最近已经游玩过了");
                    return;
                }
            }
            mapnominatelist.Add(mapname);
            Server.PrintToChatAll($"{cCSPlayer!.PlayerName} 预定了地图 {mapname}");
        }
        else
        {
            command.ReplyToCommand($"打开控制台查看地图列表");
            var x = maplist.Count;
            var y = x /10;
            var z = x - (y *10);
            for(var a=0;a<y-1;a++)
                for(var b=0;b<10;b++)
                    cCSPlayer!.PrintToConsole($"{maplist[a*10+b]}");
            if(z >0)
                for(var c = 0;c<z-1;c++)
                    cCSPlayer!.PrintToConsole($"{maplist[y*10+c]}");
        }

    }


    public void StartRtv()
    {
        KillTimer();
        Logger.LogInformation("开始投票换图");
        GetPlayersCount();
        if (playercount == 0)
        {
            isrtv = true;
            // isforcertv = true;
            var randommap = "";
            int index = random.Next(0, maplist.Count - 1);
            while (!rtvwin)
            {
                if (mapcooldown.Find(x => Regex.IsMatch(maplist[index], x)) != null)
                    continue;
                else
                {
                    randommap = maplist[index];
                    rtvwin = true;
                }
            }
            Logger.LogInformation("空服换图");
            VoteEnd(randommap);
            return;
        }
        var music = rtvmusiclist[random.Next(0, rtvmusiclist.Count - 1)];
        foreach (var player in IsPlayer())
        {
            // playClientSound(player, "UI.Guardian.TooFarWarning", 0.5f, 1f);
            AddTimer(5f, () => playClientSound(player, music, 0.5f, 1f));
        }
        if (!isrtvagain)
        {
            votemaplist = mapnominatelist;
            votemaplist.Add(Server.MapName);
            while (votemaplist.Count < 6)
            {
                int index = random.Next(0, maplist.Count - 1);
                if (votemaplist.Find(x => Regex.IsMatch(maplist[index], x)) != null || mapcooldown.Find(x => Regex.IsMatch(maplist[index], x)) != null) continue;
                votemaplist.Add(maplist[index]);
            }
        }
        ChatMenu votemenu = new ChatMenu("请从以下地图中选择一张");
        string nextmap = "";
        int totalvotes = 0;
        Dictionary<string, int> votes = new();
        votes.Clear();

        foreach (string mapname in votemaplist)
        {
            votes[mapname] = 0;
            if (mapname == Server.MapName)
            {
                votemenu.AddMenuOption("不更换地图", (player, options) =>
                {
                    votes[mapname] += 1;
                    totalvotes += 1;
                    player.PrintToChat("你已投票给不更换地图");
                    Logger.LogInformation("{PlayerName} 投票给不换图", player.PlayerName);
                    GetPlayersCount();
                    if (votes[mapname] >= rtvrequired)
                    {
                        nextmap = mapname;
                        rtvwin = true;
                        Server.PrintToChatAll("地图投票已结束");
                        VoteEnd(nextmap);
                        return;
                    }
                });
            }
            else
            {
                votemenu.AddMenuOption(mapname, (player, options) =>
                {
                    votes[mapname] += 1;
                    totalvotes += 1;
                    player.PrintToChat($"你已投票给地图 {mapname}");
                    Logger.LogInformation("{PlayerName} 投票给地图 {mapname}", player.PlayerName, mapname);
                    GetPlayersCount();
                    if (votes[mapname] >= rtvrequired)
                    {
                        nextmap = mapname;
                        rtvwin = true;
                        Server.PrintToChatAll("地图投票已结束");
                        VoteEnd(nextmap);
                        return;
                    }
                });
            }
        }

        foreach (var player in IsPlayer())
            MenuManager.OpenChatMenu(player, votemenu);

        _rtvtimer = AddTimer(30f, () =>
        {
            if (!isrtving) return;
            if (totalvotes == 0)
            {
                nextmap = mapnominatelist[random.Next(0, mapnominatelist.Count - 1)];
                Server.PrintToChatAll("地图投票已结束");
                rtvwin = true;
            }
            else if (votes.Select(x => x.Value).Max() > (totalvotes * 0.5f))
            {
                votes = votes.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
                nextmap = votes.First().Key;
                Server.PrintToChatAll("地图投票已结束");
                rtvwin = true;
            }
            else if (votes.Select(x => x.Value).Max() <= (totalvotes * 0.5f) && votemaplist.Count >= 4 && totalvotes > 2)
            {
                Server.PrintToChatAll("本轮投票未有地图投票比例超过50%，将进行下一轮投票");
                foreach (var keyValuePair in votes)
                    if (keyValuePair.Value == 0)
                        votes.Remove(keyValuePair.Key);
                votes = votes.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
                var y = votemaplist.Count();
                votemaplist.Clear();
                var x = 0;
                while (x < (y * 0.5f))
                {
                    if (votes.First().Key != null)
                    {
                        votemaplist!.Add(votes.First().Key);
                        votes.Remove(votes.First().Key);
                        x++;
                    }
                    else
                        break;
                }
            }
            else if (votes.Select(x => x.Value).Max() <= (totalvotes * 0.5f) && (votemaplist.Count < 4 || totalvotes <= 2))
            {
                votes = votes.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
                nextmap = votes.First().Key;
                if(votes.ContainsKey(Server.MapName) && votes.First().Value <= (votes.GetValueOrDefault(Server.MapName)+1))
                    nextmap = Server.MapName;
                Server.PrintToChatAll("地图投票已结束");
                rtvwin = true;
            }
            VoteEnd(nextmap);
        });
    }

    public void VoteEnd(string mapname)
    {

        if (rtvwin)
        {
            rtvwin = false;
            votemaplist.Clear();
            isrtving = false;
            isrtvagain = false;
            if (_rtvtimer != null)
            {
                _rtvtimer.Kill();
                _rtvtimer = null;
            }
            if (mapname == Server.MapName)
            {
                if (!isrtv)
                {
                    Server.PrintToChatAll("地图已延长");
                    Logger.LogInformation("地图已延长");
                    timeleft = 15;
                }
                else
                {
                    isrtv = false;
                    Server.PrintToChatAll("投票结果为不更换地图");
                    Logger.LogInformation("投票结果为不更换地图");
                }
                CanRtvtimer();
                StartMaptimer();
            }
            else
            {
                mapnominatelist.Clear();
                Logger.LogInformation("投票决定为 {mapname}", mapname);
                if (!isrtv)
                    EndMaptimer(mapname);
                else
                    ChangeMapRepeat(mapname);
            }
        }
        else
        {
            isrtvagain = true;
            var x = 0;
            _repeattimer = AddTimer(1f, () =>
            {
                x++;
                foreach (var player in IsPlayer())
                {
                    playClientSound(player, "Alert.WarmupTimeoutBeep");
                    player.PrintToChat("即将进行下一轮投票");
                }
                if(x == 10)
                    Server.NextFrame(()=>StartRtv());
            }, TimerFlags.REPEAT);
        }
    }

    private void KillTimer()
    {
        if (_canrtvtimer != null)
            _canrtvtimer.Kill();
        if (_maptimer != null)
            _maptimer.Kill();
        if (_rtvtimer != null)
            _rtvtimer.Kill();
        if (_endmaptimer != null)
            _endmaptimer.Kill();
        if (_changemaprepeat != null)
            _changemaprepeat.Kill();
        if (_repeattimer != null)
            _repeattimer.Kill();
    }
    private void GetPlayersCount()
    {
        playercount = IsPlayer().Count();
        rtvrequired = (int)Math.Ceiling(playercount * 0.6f);
    }

    private void playClientSound(CCSPlayerController ccsplayer, string sound, float volume = 1.0f, float pitch = 1.0f)
    {
        var parameters = new Dictionary<string, float>
        {
            { "volume", volume },
            { "pitch", pitch }
        };
        ccsplayer.EmitSound(sound, parameters);
    }
    private IEnumerable<CCSPlayerController> IsPlayer()
    {
        var player = Utilities.GetPlayers().Where((x) =>
        x.TeamNum > 0 &&
        x.IsValid &&
        x.Connected == PlayerConnectedState.PlayerConnected
        );
        return player;
    }

    private void StartMaptimer()
    {
        Server.NextFrame(()=>
        {
            _maptimer = AddTimer(60f, () =>
            {
                timeleft--;
                if(timeleft == 0)
                {
                    isrtving = true;
                    var x = 0;
                    _repeattimer = AddTimer(1f, () =>
                    {
                        x++;
                        foreach (var player in IsPlayer())
                        {
                            playClientSound(player, "Alert.WarmupTimeoutBeep");
                            player.PrintToChat("当前地图时长还剩5分钟");
                        }
                        if(x == 10)
                            Server.NextFrame(()=>StartRtv());
                    }, TimerFlags.REPEAT);
                }
            }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
        });
    }

    private void EndMaptimer(string mapname)
    {
        Server.NextFrame(()=>
        {
            timeleft = 5;
            _endmaptimer = AddTimer(60f, () =>
            {
                timeleft--;
                if (timeleft == 1)
                    Server.PrintToChatAll("距离换图还有60秒");
                if (timeleft == 0)
                {
                    ChangeMapRepeat(mapname);
                    Server.NextFrame(() =>
                    {
                        if (_endmaptimer != null)
                            _endmaptimer.Kill();
                    });
                }
            }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
        });
    }

    private void CanRtvtimer()
    {
        canrtv = false;
        Server.NextFrame(() =>_canrtvtimer = AddTimer(5 * 60f, () =>canrtv = true));
    }
    private void ChangeMapRepeat(string mapname)
    {
        Server.NextFrame(()=>
        {
            var music = mapendmusiclist[random.Next(0, mapendmusiclist.Count - 1)];
            var second = 10;
            _repeattimer = AddTimer(1.0f, () =>
            {
                foreach (var player in IsPlayer())
                {
                    playClientSound(player, music);
                    player.PrintToChat($"距离换图还有{second}秒");
                    if (second == 0) 
                        if(_repeattimer != null)
                            Server.NextFrame(() => _repeattimer.Kill());
                }
                second--;
            }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            var tryround = 0;
            _changemaprepeat = AddTimer(10f, () =>
            {
                Server.ExecuteCommand($"ds_workshop_changelevel {mapname}");
                tryround++;
                if (tryround > 6)
                {
                    var randommap = maplist[random.Next(0, maplist.Count - 1)];
                    Server.ExecuteCommand($"ds_workshop_changelevel {randommap}");
                }
            }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
        });
    }
}
public static class EmitSoundExtension
{
    // TODO: these are for libserver.so, haven't found these on windows yet
    private static MemoryFunctionVoid<CBaseEntity, string, int, float, float> CBaseEntity_EmitSoundParamsFunc = new("\\x48\\xB8\\x00\\x00\\x00\\x00\\x64\\x00\\x00\\x00\\x55\\x48\\x89\\xE5\\x41\\x55");
    private static MemoryFunctionWithReturn<nint, nint, uint, uint, short, ulong, ulong> CSoundOpGameSystem_StartSoundEventFunc = new("\\x48\\xB8\\x00\\x00\\x00\\x00\\x08\\x00\\x00\\xC0\\x55\\x48\\x89\\xE5\\x41\\x57\\x45");
    private static MemoryFunctionVoid<nint, nint, ulong, nint, nint, short, byte> CSoundOpGameSystem_SetSoundEventParamFunc = new("\\x55\\x48\\x89\\xE5\\x41\\x57\\x41\\x56\\x49\\x89\\xD6\\x48\\x89\\xCA\\x41\\x55\\x49");

    internal static void Init()
    {
        CSoundOpGameSystem_StartSoundEventFunc.Hook(CSoundOpGameSystem_StartSoundEventFunc_PostHook, HookMode.Post);
    }

    internal static void CleanUp()
    {
        CSoundOpGameSystem_StartSoundEventFunc.Unhook(CSoundOpGameSystem_StartSoundEventFunc_PostHook, HookMode.Post);
    }

    [ThreadStatic]
    private static IReadOnlyDictionary<string, float>? CurrentParameters;

    /// <summary>
    /// Emit a sound event by name (e.g., "Weapon_AK47.Single").
    /// TODO: parameters passed in here only seem to work for sound events shipped with the game, not workshop ones.
    /// </summary>
    public static void EmitSound(this CBaseEntity entity, string soundName, IReadOnlyDictionary<string, float>? parameters = null)
    {
        if (!entity.IsValid)
        {
            throw new ArgumentException("Entity is not valid.");
        }

        try
        {
            // We call CBaseEntity::EmitSoundParams,
            // which calls a method that returns an ID that we can use
            // to modify the playing sound.

            CurrentParameters = parameters;

            // Pitch, volume etc aren't actually used here
            CBaseEntity_EmitSoundParamsFunc.Invoke(entity, soundName, 100, 1f, 0f);
        }
        finally
        {
            CurrentParameters = null;
        }
    }

    private static HookResult CSoundOpGameSystem_StartSoundEventFunc_PostHook(DynamicHook hook)
    {
        if (CurrentParameters is not { Count: > 0 })
        {
            return HookResult.Continue;
        }

        var pSoundOpGameSystem = hook.GetParam<nint>(0);
        var pFilter = hook.GetParam<nint>(1);
        var soundEventId = hook.GetReturn<ulong>();

        foreach (var parameter in CurrentParameters)
        {
            CSoundOpGameSystem_SetSoundEventParam(pSoundOpGameSystem, pFilter,
                soundEventId, parameter.Key, parameter.Value);
        }

        return HookResult.Continue;
    }

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct FloatParamData
    {
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        private readonly uint _type1;
        private readonly uint _type2;

        private readonly uint _size1;
        private readonly uint _size2;

        private readonly float _value;
        private readonly uint _padding;
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

        public FloatParamData(float value)
        {
            _type1 = 1;
            _type2 = 8;

            _size1 = 4;
            _size2 = 4;

            _value = value;
            _padding = 0;
        }
    }

    private static unsafe void CSoundOpGameSystem_SetSoundEventParam(nint pSoundOpGameSystem, nint pFilter,
        ulong soundEventId, string paramName, float value)
    {
        var data = new FloatParamData(value);
        var nameByteCount = Encoding.UTF8.GetByteCount(paramName);

        var pData = Unsafe.AsPointer(ref data);
        var pName = stackalloc byte[nameByteCount + 1];

        Encoding.UTF8.GetBytes(paramName, new Span<byte>(pName, nameByteCount));

        CSoundOpGameSystem_SetSoundEventParamFunc.Invoke(pSoundOpGameSystem, pFilter, soundEventId, (nint)pName, (nint)pData, 0, 0);
    }
}