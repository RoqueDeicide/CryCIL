using System;
using System.Windows.Forms;
using CryEngine.Extensions;

namespace CryEngine.Sandbox
{
	internal partial class FormLoader : Form
	{
		public FormLoader()
		{
			InitializeComponent();

			// This would ideally be databound but I'm getting unexplained nullrefs - Ruan
			uxExtensionList.Items.AddRange(FormHelper.AvailableForms.ToArray());
			uxExtensionList.DisplayMember = "Name";

			uxExtensionLoad.Click += (sender, args) =>
			{
				if (uxExtensionList.SelectedItem != null)
					LoadExtension((uxExtensionList.SelectedItem as FormInfo).Type);
			};
		}

		private void OnExtensionSelect(object sender, EventArgs e)
		{
			var formInfo = uxExtensionList.SelectedItem as FormInfo;
			var data = formInfo.Data;

			uxExtensionInfo.Clear();
			uxExtensionInfo.Append("Name: {0}", data.Name);

			uxExtensionInfo.NewLine();

			if (!string.IsNullOrEmpty(data.AuthorName))
				uxExtensionInfo.Append("Developed by {0}", data.AuthorName);

			if (!string.IsNullOrEmpty(data.AuthorContact))
				uxExtensionInfo.Append(" ({0})", data.AuthorContact);

			uxExtensionInfo.NewLine(2);

			if (!string.IsNullOrEmpty(data.Description))
				uxExtensionInfo.Append(data.Description);
		}

		private void LoadExtension(Type type)
		{
			var form = Activator.CreateInstance(type, null) as Form;
			form.Show();
		}
	}
}