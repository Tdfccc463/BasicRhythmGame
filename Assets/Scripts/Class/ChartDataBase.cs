//谱面数据库，根据谱面序号索引到谱面
public class ChartDatabase
{
    [System.Serializable]
    public class Chart
    {
        public int ChartId;
        public string ChartPath;
    }
    public Chart[] Charts;
    public Chart GetChart(int id)
    {
        foreach (var chart in Charts)
        {
            if (chart.ChartId == id)
            {
                return chart;
            }
        }
        return default;
    }
}