using System;
using System.Collections.Generic;

namespace EFPerformance.Models;

public partial class IxUsageReport
{
    public string? TabloAdi { get; set; }

    public string? IndexAdi { get; set; }

    public string? TipAciklamasi { get; set; }

    public long SeekSayisi { get; set; }

    public long ScanSayisi { get; set; }

    public long LookupSayisi { get; set; }

    public long GuncellemeMaaliyeti { get; set; }

    public DateTime? LastUserSeek { get; set; }

    public DateTime? LastUserScan { get; set; }
}
