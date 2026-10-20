using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using APIchecklistProcess.Model.CommonFunction;

public partial class UC_SelectPartNumber : UserControl
{
    private int UnitIndex;
    private List<string> partNumbersinfor = new List<string>();
    string filePath = @"C:\Aavid_Test\Setup-ini\Flushing_Part_Numbers.txt";
    private string[] allLine;
    public string OperatorID { get; private set; }
    public UC_SelectPartNumber(int unitIndex)
    {
        InitializeComponent();
        UnitIndex = unitIndex;
        // Gọi async load part numbers khi control được tạo
        _ = LoadPartNumbersAsync();
       
    }

    /// <summary>
    /// Tải danh sách Part Number qua API (hybrid: API-first + DB fallback tự động).
    /// </summary>
    private async Task LoadPartNumbersAsync()
    {
        try
        {
            partNumbersinfor = await GetPartNumberInforAsync();
            LoadPartNumbersIntoComboBox();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading part numbers: {ex.Message}");
            partNumbersinfor = new List<string>();
        }
    }

    /// <summary>
    /// Lấy danh sách Part Number qua API.commonFunctionService.GetAllPartNumbersAsync()
    /// (hybrid: API-first, tự động fallback sang truy vấn DB trực tiếp nếu API có vấn đề kết nối).
    /// </summary>
    private async Task<List<string>> GetPartNumberInforAsync()
    {
        List<string> result = new List<string>();

        PartNumberListResponse response = await API.commonFunctionService.GetAllPartNumbersAsync();

        if (response != null && response.Success && response.Data != null)
        {
            foreach (var item in response.Data)
            {
                string displayText = $"{item.AssyPN} - {item.Description}";
                result.Add(displayText);
            }
        }

        return result;
    }
    private void LoadPartNumbersIntoComboBox()
    {
        this.lb_Fixture.Text = "Fixture " + (UnitIndex + 1).ToString();
        cbb_PartNumberInfo.Items.AddRange(partNumbersinfor.ToArray());
        try
        {
            allLine = File.ReadAllLines(@"C:\Aavid_Test\Setup-ini\Flushing_Part_Numbers.txt");
            string[] infor = allLine[UnitIndex + 1].Split(',');
            txt_WO.Text = infor[2];
            txt_PN.Text = infor[1];

            for (int i = 0; i < partNumbersinfor.Count; i++)
            {
                if (partNumbersinfor[i].Contains(infor[1]))
                {
                    cbb_PartNumberInfo.SelectedIndex = i;
                    txt_Description.Text = partNumbersinfor[i].Split('-')[1].Trim();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error when get line text from C:\\Aavid_Test\\Setup-ini\\Flushing_Part_Numbers.txt: " + ex.Message);
        }
    }
    private void UC_SelectPartNumber_Load(object sender, EventArgs e)
    {
        
    }

    private void cbb_PartNumberInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedPartNumber = cbb_PartNumberInfo.SelectedItem.ToString();
        txt_PN.Text = partNumbersinfor[cbb_PartNumberInfo.SelectedIndex].Split('-')[0].Trim();
        txt_Description.Text = partNumbersinfor[cbb_PartNumberInfo.SelectedIndex].Split('-')[1].Trim();
    }

    private void btn_ChangePart_Click(object sender, EventArgs e)
    {
        if (!File.Exists(filePath))
        {
            string DirectoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("Fixture_Id,Part_Number,Work_Order,T2_Off,Nest_Enable,");
                sw.WriteLine("708966-TF-001,708966,3121369369,0,Enabled,");
                sw.WriteLine("708964-TF-002,708964,,0,Enabled,");
                sw.WriteLine("708964-TF-003,708964,,0,Enabled,");
            }
        }    
        string[] allLine = File.ReadAllLines(@"C:\Aavid_Test\Setup-ini\Flushing_Part_Numbers.txt").Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        // Nếu ít hơn 4 phần tử thì tăng lên 4 bằng cách thêm dòng trống
        if (allLine.Length < 5)
        {
            List<string> lines = allLine.ToList();
            while (lines.Count < 5)
            {
                lines.Add(""); // hoặc có thể là "N/A", "None", v.v.
            }
            allLine = lines.ToArray();
        }
        if (!CheckWorkOrder(txt_WO.Text.Trim()))
        {
            return;
        }
        if(!CheckOperatorID(txtOP.Text.Trim()))
        {
            return;
        }
        OperatorID = txtOP.Text.Trim();
        string[] newLine = new string[] { $"{txt_PN.Text.Trim()}-TF-00{(UnitIndex+1).ToString()}", txt_PN.Text.Trim(), txt_WO.Text.Trim(), "0", "Enabled" };
        allLine[UnitIndex+1] = string.Join(",", newLine) + ",";
        // Ghi lại file
        File.WriteAllLines(filePath, allLine);
    }
    private bool CheckWorkOrder(string workOrder)
    {
        if (string.IsNullOrWhiteSpace(workOrder))
        {
            MessageBox.Show("Work Order cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        if (!workOrder.All(char.IsDigit))
        {
            MessageBox.Show("Work Order must contain only digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        if (workOrder.Length != 10)
        {
            MessageBox.Show("Work Order must be exactly 10 digits long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        if (!workOrder.StartsWith("3121"))
        {
            MessageBox.Show("Work Order must start with '3121'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }
    private bool CheckOperatorID(string operatorID)
    {
        if (string.IsNullOrWhiteSpace(operatorID))
        {
            MessageBox.Show("Operator ID cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        if(!operatorID.StartsWith("B"))
        {
            MessageBox.Show("Operator ID must start with 'B'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }    
        if(operatorID.Substring(operatorID.IndexOf("B")+1).Length != 5)
        {
            MessageBox.Show("Operator ID must be exactly 5 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        if(!operatorID.Substring(operatorID.IndexOf("B") + 1).All(char.IsDigit))
        {
            MessageBox.Show("Operator ID must contain only digits after 'B'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        return true;
    }
}
