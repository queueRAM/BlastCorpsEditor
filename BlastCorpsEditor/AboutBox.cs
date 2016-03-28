using System;
using System.Reflection;
using System.Windows.Forms;

namespace BlastCorpsEditor
{
   partial class AboutBox : Form
   {
      public AboutBox()
      {
         var thanks = "Special thanks to :\r\n" +
            "  \u2022 SunakazeKun / Aurum for Blast Corps documentation and testing\r\n" +
            "  \u2022 SubDrag for the Universal N64 Compressor and notes\r\n" +
            "  \u2022 Everyone else who has helped along the way";
         InitializeComponent();
         this.Text = String.Format("About {0}", AssemblyTitle);
         this.labelProductName.Text = AssemblyProduct;
         this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
         this.labelCopyright.Text = AssemblyCopyright;
         this.textBoxDescription.Text = thanks;
      }

      #region Assembly Attribute Accessors

      public string AssemblyTitle
      {
         get
         {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
               AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
               if (titleAttribute.Title != "")
               {
                  return titleAttribute.Title;
               }
            }
            return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
         }
      }

      public string AssemblyVersion
      {
         get
         {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
         }
      }

      public string AssemblyDescription
      {
         get
         {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length == 0)
            {
               return "";
            }
            return ((AssemblyDescriptionAttribute)attributes[0]).Description;
         }
      }

      public string AssemblyProduct
      {
         get
         {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
               return "";
            }
            return ((AssemblyProductAttribute)attributes[0]).Product;
         }
      }

      public string AssemblyCopyright
      {
         get
         {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length == 0)
            {
               return "";
            }
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
         }
      }

      public string AssemblyCompany
      {
         get
         {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
            {
               return "";
            }
            return ((AssemblyCompanyAttribute)attributes[0]).Company;
         }
      }
      #endregion
   }
}
