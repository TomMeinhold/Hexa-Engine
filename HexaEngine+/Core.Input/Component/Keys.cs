#region Assembly System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089

// C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Windows.Forms.dll

#endregion Assembly System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HexaEngine.Core.Input.Component
{
    // Zusammenfassung: Stellt Tastencodes und Modifizierer bereit.
    [ComVisible(true)]
    [Editor("System.Windows.Forms.Design.ShortcutKeysEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
    [Flags]
    [TypeConverter(typeof(KeysConverter))]
    public enum Keys
    {
        // Zusammenfassung: Die Bitmaske zum Extrahieren von Modifizierern aus einem Schlüssel-Wert.
        Modifiers = -65536,

        // Zusammenfassung: Keine Taste gedrückt.
        None = 0,

        // Zusammenfassung: Die linke Maustaste gedrückt.
        LButton = 1,

        // Zusammenfassung: Die rechte Maustaste gedrückt.
        RButton = 2,

        // Zusammenfassung: Der Schlüssel "Abbrechen".
        Cancel = 3,

        // Zusammenfassung: Die mittlere Maustaste (drei Maustaste).
        MButton = 4,

        // Zusammenfassung: Die erste x los (fünf Schaltflächen Maus).
        XButton1 = 5,

        // Zusammenfassung: Die zweite x los (fünf Schaltflächen Maus).
        XButton2 = 6,

        // Zusammenfassung: RÜCKTASTE
        Back = 8,

        // Zusammenfassung: TAB-TASTE
        Tab = 9,

        // Zusammenfassung: ZEILENVORSCHUBTASTE
        LineFeed = 10,

        // Zusammenfassung: ENTF-TASTE
        Clear = 12,

        // Zusammenfassung: RETURN-Taste.
        Return = 13,

        // Zusammenfassung: EINGABETASTE
        Enter = 13,

        // Zusammenfassung: Die UMSCHALTTASTE gedrückt.
        ShiftKey = 16,

        // Zusammenfassung: Die STRG-Taste.
        ControlKey = 17,

        // Zusammenfassung: Die ALT-Taste.
        Menu = 18,

        // Zusammenfassung: PAUSE-TASTE
        Pause = 19,

        // Zusammenfassung: Die Feststelltaste.
        Capital = 20,

        // Zusammenfassung: Die Feststelltaste.
        CapsLock = 20,

        // Zusammenfassung: Taste für den IME-Kana-Modus
        KanaMode = 21,

        // Zusammenfassung: Der Schlüssel für den IME-Hanguel-Modus. (Kompatibilitätsgründen
        // beibehalten; verwenden Sie HangulMode)
        HanguelMode = 21,

        // Zusammenfassung: Taste für den IME-Hangul-Modus
        HangulMode = 21,

        // Zusammenfassung: Taste für den IME-Junja-Modus
        JunjaMode = 23,

        // Zusammenfassung: Der Schlüssel für den IME-final-Modus.
        FinalMode = 24,

        // Zusammenfassung: Taste für den IME-Hanja-Modus
        HanjaMode = 25,

        // Zusammenfassung: Taste für den IME-Kanji-Modus
        KanjiMode = 25,

        // Zusammenfassung: ESC-TASTE
        Escape = 27,

        // Zusammenfassung: Der Schlüssel für den IME-konvertieren.
        IMEConvert = 28,

        // Zusammenfassung: Die Taste für IME
        IMENonconvert = 29,

        // Zusammenfassung: Das annehmen im IME Schlüssel ersetzt System.Windows.Forms.Keys.IMEAceept.
        IMEAccept = 30,

        // Zusammenfassung: Das annehmen im IME Schlüssel. Veraltet, verwenden Sie
        // System.Windows.Forms.Keys.IMEAccept stattdessen.
        IMEAceept = 30,

        // Zusammenfassung: Der IME-Modus Änderungsschlüssel.
        IMEModeChange = 31,

        // Zusammenfassung: LEERTASTE
        Space = 32,

        // Zusammenfassung: BILD-AUF-TASTE
        Prior = 33,

        // Zusammenfassung: BILD-AUF-TASTE
        PageUp = 33,

        // Zusammenfassung: BILD-AB-TASTE
        Next = 34,

        // Zusammenfassung: BILD-AB-TASTE
        PageDown = 34,

        // Zusammenfassung: ENDE-TASTE
        End = 35,

        // Zusammenfassung: POS1-TASTE
        Home = 36,

        // Zusammenfassung: NACH-LINKS-TASTE
        Left = 37,

        // Zusammenfassung: NACH-OBEN-TASTE
        Up = 38,

        // Zusammenfassung: NACH-RECHTS-TASTE
        Right = 39,

        // Zusammenfassung: NACH-UNTEN-TASTE
        Down = 40,

        // Zusammenfassung: AUSWAHL-TASTE
        Select = 41,

        // Zusammenfassung: DRUCKEN-TASTE
        Print = 42,

        // Zusammenfassung: AUSFÜHREN-TASTE
        Execute = 43,

        // Zusammenfassung: DRUCK-TASTE
        Snapshot = 44,

        // Zusammenfassung: DRUCK-TASTE
        PrintScreen = 44,

        // Zusammenfassung: EINFG-Taste.
        Insert = 45,

        // Zusammenfassung: Die ENTF-Taste.
        Delete = 46,

        // Zusammenfassung: HILFE-TASTE
        Help = 47,

        // Zusammenfassung: 0-TASTE
        D0 = 48,

        // Zusammenfassung: 1-TASTE
        D1 = 49,

        // Zusammenfassung: 2-TASTE
        D2 = 50,

        // Zusammenfassung: 3-TASTE
        D3 = 51,

        // Zusammenfassung: 4-TASTE
        D4 = 52,

        // Zusammenfassung: 5-TASTE
        D5 = 53,

        // Zusammenfassung: 6-TASTE
        D6 = 54,

        // Zusammenfassung: 7-TASTE
        D7 = 55,

        // Zusammenfassung: 8-TASTE
        D8 = 56,

        // Zusammenfassung: 9-TASTE
        D9 = 57,

        // Zusammenfassung: A-TASTE
        A = 65,

        // Zusammenfassung: B-TASTE
        B = 66,

        // Zusammenfassung: C-TASTE
        C = 67,

        // Zusammenfassung: D-TASTE
        D = 68,

        // Zusammenfassung: E-TASTE
        E = 69,

        // Zusammenfassung: F-TASTE
        F = 70,

        // Zusammenfassung: G-TASTE
        G = 71,

        // Zusammenfassung: H-TASTE
        H = 72,

        // Zusammenfassung: I-TASTE
        I = 73,

        // Zusammenfassung: J-TASTE
        J = 74,

        // Zusammenfassung: K-TASTE
        K = 75,

        // Zusammenfassung: L-TASTE
        L = 76,

        // Zusammenfassung: M-TASTE
        M = 77,

        // Zusammenfassung: N-TASTE
        N = 78,

        // Zusammenfassung: O-TASTE
        O = 79,

        // Zusammenfassung: P-TASTE
        P = 80,

        // Zusammenfassung: Q-TASTE
        Q = 81,

        // Zusammenfassung: R-TASTE
        R = 82,

        // Zusammenfassung: S-TASTE
        S = 83,

        // Zusammenfassung: T-TASTE
        T = 84,

        // Zusammenfassung: U-TASTE
        U = 85,

        // Zusammenfassung: V-TASTE
        V = 86,

        // Zusammenfassung: W-TASTE
        W = 87,

        // Zusammenfassung: X-TASTE
        X = 88,

        // Zusammenfassung: Y-TASTE
        Y = 89,

        // Zusammenfassung: Z-TASTE
        Z = 90,

        // Zusammenfassung: Linke Windows-Taste (Microsoft Natural Keyboard)
        LWin = 91,

        // Zusammenfassung: Rechte Windows-Taste (Microsoft Natural Keyboard)
        RWin = 92,

        // Zusammenfassung: Der Anwendungsschlüssel (Microsoft Natural Keyboard).
        Apps = 93,

        // Zusammenfassung: Die Taste für Standbymodus.
        Sleep = 95,

        // Zusammenfassung: 0-TASTE auf der Zehnertastatur
        NumPad0 = 96,

        // Zusammenfassung: 1-TASTE auf der Zehnertastatur
        NumPad1 = 97,

        // Zusammenfassung: 2-TASTE auf der Zehnertastatur
        NumPad2 = 98,

        // Zusammenfassung: 3-TASTE auf der Zehnertastatur
        NumPad3 = 99,

        // Zusammenfassung: 4-TASTE auf der Zehnertastatur
        NumPad4 = 100,

        // Zusammenfassung: 5-TASTE auf der Zehnertastatur
        NumPad5 = 101,

        // Zusammenfassung: 6-TASTE auf der Zehnertastatur
        NumPad6 = 102,

        // Zusammenfassung: 7-TASTE auf der Zehnertastatur
        NumPad7 = 103,

        // Zusammenfassung: 8-TASTE auf der Zehnertastatur
        NumPad8 = 104,

        // Zusammenfassung: 9-TASTE auf der Zehnertastatur
        NumPad9 = 105,

        // Zusammenfassung: Multiplikationstaste
        Multiply = 106,

        // Zusammenfassung: Der Schlüssel hinzufügen.
        Add = 107,

        // Zusammenfassung: TRENNZEICHENTASTE
        Separator = 108,

        // Zusammenfassung: SUBTRAKTIONSTASTE
        Subtract = 109,

        // Zusammenfassung: KOMMATASTE
        Decimal = 110,

        // Zusammenfassung: Divisionstaste
        Divide = 111,

        // Zusammenfassung: F1-TASTE
        F1 = 112,

        // Zusammenfassung: F2-TASTE
        F2 = 113,

        // Zusammenfassung: F3-TASTE
        F3 = 114,

        // Zusammenfassung: F4-TASTE
        F4 = 115,

        // Zusammenfassung: F5-TASTE
        F5 = 116,

        // Zusammenfassung: F6-TASTE
        F6 = 117,

        // Zusammenfassung: F7-TASTE
        F7 = 118,

        // Zusammenfassung: F8-TASTE
        F8 = 119,

        // Zusammenfassung: F9-TASTE
        F9 = 120,

        // Zusammenfassung: F10-TASTE
        F10 = 121,

        // Zusammenfassung: F11-TASTE
        F11 = 122,

        // Zusammenfassung: F12-TASTE
        F12 = 123,

        // Zusammenfassung: F13-TASTE
        F13 = 124,

        // Zusammenfassung: F14-TASTE
        F14 = 125,

        // Zusammenfassung: F15-TASTE
        F15 = 126,

        // Zusammenfassung: F16-TASTE
        F16 = 127,

        // Zusammenfassung: F17-TASTE
        F17 = 128,

        // Zusammenfassung: F18-TASTE
        F18 = 129,

        // Zusammenfassung: F19-TASTE
        F19 = 130,

        // Zusammenfassung: F20-TASTE
        F20 = 131,

        // Zusammenfassung: F21-TASTE
        F21 = 132,

        // Zusammenfassung: F22-TASTE
        F22 = 133,

        // Zusammenfassung: F23-TASTE
        F23 = 134,

        // Zusammenfassung: F24-TASTE
        F24 = 135,

        // Zusammenfassung: NUM-Taste.
        NumLock = 144,

        // Zusammenfassung: Rollen-Taste.
        Scroll = 145,

        // Zusammenfassung: Linke UMSCHALTTASTE.
        LShiftKey = 160,

        // Zusammenfassung: Rechte UMSCHALTTASTE.
        RShiftKey = 161,

        // Zusammenfassung: Linke STRG-TASTE
        LControlKey = 162,

        // Zusammenfassung: Rechte STRG-TASTE
        RControlKey = 163,

        // Zusammenfassung: Linke ALT-TASTE
        LMenu = 164,

        // Zusammenfassung: Rechte ALT-TASTE
        RMenu = 165,

        // Zusammenfassung: Der Browser zurück-Taste (Windows 2000 oder höher).
        BrowserBack = 166,

        // Zusammenfassung: Der Browser vorwärts-Taste (Windows 2000 oder höher).
        BrowserForward = 167,

        // Zusammenfassung: Die Browser-aktualisieren-Taste (Windows 2000 oder höher).
        BrowserRefresh = 168,

        // Zusammenfassung: Die Browser-Stop-Taste (Windows 2000 oder höher).
        BrowserStop = 169,

        // Zusammenfassung: Die Browser-suchen-Taste (Windows 2000 oder höher).
        BrowserSearch = 170,

        // Zusammenfassung: Die Browser-Favoriten-Taste (Windows 2000 oder höher).
        BrowserFavorites = 171,

        // Zusammenfassung: Der Browser POS1-Taste (Windows 2000 oder höher).
        BrowserHome = 172,

        // Zusammenfassung: Der Datenträger Schlüssel Stummschalten (Windows 2000 oder höher).
        VolumeMute = 173,

        // Zusammenfassung: Leiser-Taste (Windows 2000 oder höher).
        VolumeDown = 174,

        // Zusammenfassung: Lauter-Taste (Windows 2000 oder höher).
        VolumeUp = 175,

        // Zusammenfassung: Das Medium für den nächsten Titel (Windows 2000 oder höher).
        MediaNextTrack = 176,

        // Zusammenfassung: Die vorherigen Playertaste Titel (Windows 2000 oder höher).
        MediaPreviousTrack = 177,

        // Zusammenfassung: Der Media-Stop-Schlüssel (Windows 2000 oder höher).
        MediaStop = 178,

        // Zusammenfassung: Das Medium wiederzugeben Pause-Taste (Windows 2000 oder höher).
        MediaPlayPause = 179,

        // Zusammenfassung: Der Start-Mail-Schlüssel (Windows 2000 oder höher).
        LaunchMail = 180,

        // Zusammenfassung: Der Schlüssel der Medienauswahl (Windows 2000 oder höher).
        SelectMedia = 181,

        // Zusammenfassung: Der Start einen Anwendungsschlüssel (Windows 2000 oder höher).
        LaunchApplication1 = 182,

        // Zusammenfassung: Der Start der Anwendung zwei Schlüssel (Windows 2000 oder höher).
        LaunchApplication2 = 183,

        // Zusammenfassung: Die OEM-Abhängige SEMIKOLONTASTE auf US-Standardtastatur (Windows 2000
        // oder höher).
        OemSemicolon = 186,

        // Zusammenfassung: OEM 1-TASTE
        Oem1 = 186,

        // Zusammenfassung: Die OEM-Abhängige Plustaste Land/Region auf Tastatur für beliebiges
        // (Windows 2000 oder höher).
        Oemplus = 187,

        // Zusammenfassung: Die OEM-Abhängige KOMMATASTE Land/Region auf Tastatur für beliebiges
        // (Windows 2000 oder höher).
        Oemcomma = 188,

        // Zusammenfassung: OEM-Abhängige Minustaste Land/Region auf Tastatur für beliebiges
        // (Windows 2000 oder höher).
        OemMinus = 189,

        // Zusammenfassung: Der Zeitraum OEM-Schlüssel Land/Region auf Tastatur für beliebiges
        // (Windows 2000 oder höher).
        OemPeriod = 190,

        // Zusammenfassung: Der OEM-Fragezeichen-Schlüssel auf einem US-Standardtastatur (Windows
        // 2000 oder höher).
        OemQuestion = 191,

        // Zusammenfassung: OEM 2-TASTE
        Oem2 = 191,

        // Zusammenfassung: Die OEM-Abhängige TILDETASTE auf US-Standardtastatur (Windows 2000 oder höher).
        Oemtilde = 192,

        // Zusammenfassung: OEM 3-TASTE
        Oem3 = 192,

        // Zusammenfassung: Die OEM-Schlüssel der öffnenden Klammer auf US-Standardtastatur (Windows
        // 2000 oder höher).
        OemOpenBrackets = 219,

        // Zusammenfassung: OEM 4-TASTE
        Oem4 = 219,

        // Zusammenfassung: Der OEM-Pipe-Schlüssel auf einem US-Standardtastatur (Windows 2000 oder höher).
        OemPipe = 220,

        // Zusammenfassung: OEM 5-TASTE
        Oem5 = 220,

        // Zusammenfassung: Die OEM-Schlüssel der schließenden Klammer auf US-Standardtastatur
        // (Windows 2000 oder höher).
        OemCloseBrackets = 221,

        // Zusammenfassung: OEM 6-TASTE
        Oem6 = 221,

        // Zusammenfassung: Die OEM singled/doppelte Anführungszeichen Schlüssel auf einem
        // US-Standardtastatur (Windows 2000 oder höher).
        OemQuotes = 222,

        // Zusammenfassung: OEM 7-TASTE
        Oem7 = 222,

        // Zusammenfassung: OEM 8-TASTE
        Oem8 = 223,

        // Zusammenfassung: Die OEM-Abhängige spitze Klammer oder umgekehrten Schrägstrich-Taste auf
        // der RT-102-Tastatur (Windows 2000 oder höher).
        OemBackslash = 226,

        // Zusammenfassung: OEM 102-TASTE
        Oem102 = 226,

        // Zusammenfassung: Der Prozess Schlüssel.
        ProcessKey = 229,

        // Zusammenfassung: Verwendet, um Unicode-Zeichen zu übergeben, als wären sie
        // Tastatureingaben. Der Schlüsselwert des Pakets wird das niedrige Word des eine
        // 32-Bit-virtual-Schlüssel-Wert, der für nicht-Tastatur Eingabemethoden verwendet.
        Packet = 231,

        // Zusammenfassung: ATTN-TASTE
        Attn = 246,

        // Zusammenfassung: CRSEL-TASTE
        Crsel = 247,

        // Zusammenfassung: EXSEL-TASTE
        Exsel = 248,

        // Zusammenfassung: ERASE EOF-TASTE
        EraseEof = 249,

        // Zusammenfassung: PLAY-TASTE
        Play = 250,

        // Zusammenfassung: ZOOM-TASTE
        Zoom = 251,

        // Zusammenfassung: Für zukünftige Verwendung reservierte Konstante
        NoName = 252,

        // Zusammenfassung: PA1-TASTE
        Pa1 = 253,

        // Zusammenfassung: ENTF-TASTE
        OemClear = 254,

        // Zusammenfassung: Die Bitmaske zum Extrahieren eines Tastencodes aus einem Schlüssel-Wert.
        KeyCode = 65535,

        // Zusammenfassung: Die Modifizierer UMSCHALTTASTE.
        Shift = 65536,

        // Zusammenfassung: Die Modifizierer STRG-Taste.
        Control = 131072,

        // Zusammenfassung: Die ALT-Modifizierertaste.
        Alt = 262144
    }
}