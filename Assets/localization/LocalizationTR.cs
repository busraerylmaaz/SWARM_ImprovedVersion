using System.Collections.Generic;
using UnityEngine;

public class LocalizationTR : ILocalization
{
    private readonly Dictionary<string, string> localizedText = new Dictionary<string, string>
    {
        { "menu_start", "BAŞLAT" },
{ "menu_settings", "AYARLAR" },
{ "menu_exit", "ÇIKIŞ" },
{ "menu_languages", "DİL" },
{ "menu_resume", "DEVAM ET" },
{ "menu_restart", "YENİDEN BAŞLAT" },
{ "menu_music", "MÜZİK" },
{ "menu_support", "DESTEK" },
{ "menu_audio", "SES" },
{ "back", "GERİ" },
{ "menu_continue", "DEVAM ET" },
{ "assault_rifle_name", "HÜCUM TÜFEĞİ" },
{ "assault_rifle_description", "HER SAVAŞ MESAFESİ İÇİN ÇOK YÖNLÜ BİR TÜFEK." },
{ "shotgun_name", "POMPALI TÜFEK" },
{ "shotgun_description", "YAKIN MESAFEDE YÜKSEK HASAR İÇİN GÜÇLÜ BİR SİLAH." },
{ "grenade_launcher_name", "EL BOMBASI FIRLATICI" },
{ "grenade_launcher_description", "DÜŞMAN GRUPLARINI VURMAK İÇİN PATLAYICILAR FIRLATIR." },
{ "sniper_name", "KESKİN NİŞANCI TÜFEĞİ" },
{ "sniper_description", "DELİCİ MERMİLER ATAN GÜÇLÜ BİR SİLAH." },
{ "health_skill_name", "ÜSTÜN SAĞLIK" },
{ "health_skill_description", "MAKSİMUM SAĞLIĞINIZI ARTIRIR." },
{ "speed-skill_name", "IŞIK HIZI" },
{ "speed-skill_description", "DAHA HIZLI HAREKET ETMENİZİ SAĞLAR." },
{ "damage-skill_name", "ÖLÜMCÜL HASAR" },
{ "damage-skill_description", "SALDIRI GÜCÜNÜZÜ ARTIRIR." },
{ "dodge-skill_name", "KAÇINMA BECERİSİ" },
{ "dodge-skill_description", "SALDIRILARDAN KAÇINMA ŞANSINIZI ARTIRIR." },
{ "firerate-skill_name", "ATIŞ HIZI BECERİSİ" },
{ "firerate-skill_description", "SİLAHINIZIN ATEŞ HIZINI ARTIRIR." },
{ "maxammo-skill_name", "MAKSİMUM CEPHANE BECERİSİ" },
{ "maxammo-skill_description", "MAKSİMUM CEPHANE KAPASİTENİZİ ARTIRIR." },
{ "slide_hint", "SHIFT TUŞUYLA DÜŞMANLARDAN DAHA KOLAY KAÇABİLİRSİN! AMA KURTULMAK SANDIĞIN KADAR KOLAY OLMAYACAK..." },
{ "continue", "DEVAM" },
{ "level_summary", "SEVİYE ÖZETİ" },
{ "victory", "ZAFER!" },
{ "defeat", "YENİLGİ!" },
{ "character", "KARAKTER" },
{ "gun", "SİLAH" },
{ "mobs_killed", "ÖLDÜRÜLEN DÜŞMANLAR" },
{ "time_remaining", "KALAN SÜRE" },
{ "market", "PAZAR" },
{ "retry", "TEKRAR DENE" },
{ "nox_summary", "HIZLI HAREKET EDEN, HAFİF ZIRHLI BİR KADIN NİŞANCI. DAHA YÜKSEK HIZ VE ÇEVİKLİK, DAHA HIZLI ATIŞ HIZI AMA DAHA DÜŞÜK SAVUNMA SUNAR." },
{ "ares_summary", "AĞIR ZIRHLI BİR ERKEK SAVAŞÇI. DAHA DÜŞÜK HIZ AMA DAHA FAZLA SAĞLIK. DAHA DÜŞÜK ATIŞ HIZI AMA DAHA YÜKSEK SAVUNMA SUNAR." },
{ "modifiers", "DEĞİŞTİRİCİLER" },
{ "player", "OYUNCU" },
{ "RifleHolder", "ASSAULT RIFLE" },
{ "ShotgunHolder", "SHOTGUN" },
{ "GrenadelauncherHolder", "GRENADE LAUNCHER" },
{ "SniperHolder", "SNIPER" }
    };

    public string GetValue(string key)
    {
        if (localizedText.ContainsKey(key))
            return localizedText[key];

        Debug.LogWarning($"Anahtar bulunamadi: {key}");
        return key;
    }
}
