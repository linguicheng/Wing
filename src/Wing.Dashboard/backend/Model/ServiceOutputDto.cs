namespace Wing.Dashboard.Model
{
    public class ServiceOutputDto
    {
        public string Name { get; set; }

        public int Total { get; set; }

        public int HealthyTotal { get; set; }

        public double HealthyLv { get; set; }

        public int CriticalTotal { get; set; }

        public double CriticalLv { get; set; }

        public int WarningTotal { get; set; }

        public double WarningLv { get; set; }

        public int MaintenanceTotal { get; set; }

        public double MaintenanceLv { get; set; }
    }
}
