using System;

namespace local.models
{
	public class RaderGraphModel
	{
		private double[] _graph_values;

		private int[] _disp_values;

		private double[] _raw_values;

		public double[] GraphValues => _graph_values;

		public int[] DispValues => _disp_values;

		public double[] RawValues => _raw_values;

		public RaderGraphModel(ShipModel[] ships)
		{
			_graph_values = new double[5];
			_disp_values = new int[5];
			_raw_values = new double[5];
			for (int i = 0; i < ships.Length; i++)
			{
				_raw_values[0] += ships[i].Karyoku;
				_raw_values[1] += ships[i].Raisou;
				_raw_values[2] += ships[i].Taiku;
				_raw_values[3] += ships[i].Kaihi;
				_raw_values[4] += ships[i].MaxHp;
			}
			if (ships.Length > 0)
			{
				double num = Math.Sqrt(ships.Length - 1);
				for (int j = 0; j < 5; j++)
				{
					int num2 = (int)Math.Round(_raw_values[j] / (double)ships.Length);
					int num3 = (int)Math.Round((double)num2 * num);
					_disp_values[j] = num2 + num3;
					_graph_values[j] = (double)_disp_values[j] * 100.0 / 350.0;
				}
			}
		}

		public override string ToString()
		{
			string empty = string.Empty;
			return empty + $"火力:{_disp_values[0]}({_graph_values[0]:f}) 雷装:{_disp_values[1]}({_graph_values[1]:f}) 対空:{_disp_values[2]}({_graph_values[2]:f}) 回避:{_disp_values[3]}({_graph_values[3]:f}) 耐久:{_disp_values[4]}({_graph_values[4]:f})";
		}
	}
}
