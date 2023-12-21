using System;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;
using UnityEditor;

//��Ԫ����Ϣ
public struct CellInfo
{
    public string Type;//����
    public string Name;//����
    public string Description;//����
}

public static class ExcelExporterEditor
{
    // Excel���·��
    private const string ExcelPath = "./Assets/Resources/Excels/";

    // ������Json�ļ� ����Unity�е�·��
    private const string clientPath = "./Assets/StreamingAssets/Config/";

    private const int CommentsLine = 0;//ע����
    private const int VariableNameLine = 1;//��������
    private const int VariableTypeLine = 2;//����������
    private const int DataLine = 3;//����������

    [MenuItem("Tools/����Excel���ñ�")]
    private static void ExportConfigs()
    {
        try
        {
            //����excel��
            ExportConfigs(clientPath);
            //������Ӧ��ʵ��ṹ��
            ExportAllCalss(@"./Assets/Scripts/Config/", "");

            AssetDatabase.Refresh();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private static void ExportConfigs(string exportDir)
    {
        //�����洢�����ļ���
        foreach (string filePath in Directory.GetFiles(ExcelPath))
        {
            //����ļ���չ������.xlse ˵�����Ǳ���ļ� ����������һ��
            if (Path.GetExtension(filePath) != ".xlsx") continue;
            //�����ʼ��~ ˵���ǻ����ļ� ����������һ��
            if (Path.GetFileName(filePath).StartsWith("~")) continue;

            string fileName = Path.GetFileName(filePath);
            Debug.Log("fileName:" + fileName);

            Export(filePath, exportDir);
        }
    }

    // �����������ñ�Ϊcs�ļ�
    private static void ExportAllCalss(string exportDir, string csHead)
    {
        foreach (var filePath in Directory.GetFiles(ExcelPath))
        {
            if (Path.GetExtension(filePath) != ".xlsx") continue;
            if (Path.GetFileName(filePath).StartsWith("~")) continue;

            ExportClass(filePath, exportDir, csHead);
        }

        AssetDatabase.Refresh();
    }

    // �����������ñ�Ϊcs�ļ�
    private static void ExportClass(string fileName, string exportDir, string csHead)
    {
        // �������Ķ���
        XSSFWorkbook xssfWorkbook;
        using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            xssfWorkbook = new XSSFWorkbook(file);
        }
        // �ļ��� ������׺
        string protoName = Path.GetFileNameWithoutExtension(fileName);
        // �����ļ�·��
        string exportPath = Path.Combine(exportDir, $"{protoName}.cs");
        using (FileStream txt = new FileStream(exportPath, FileMode.Create))
        using (StreamWriter sw = new StreamWriter(txt))
        {
            StringBuilder sb = new StringBuilder();
            // ��ȡ��һ��excel��
            ISheet sheet = xssfWorkbook.GetSheetAt(0);
            // ������Ҫ�����ĸ�ʽ
            sb.Append("using System.Collections.Generic;\t\n");
            sb.Append($"public class {protoName}s\n");//����
            sb.Append("{\n");
            sb.Append($"\tpublic List<{protoName}> info;\n");
            sb.Append("}\n\n");


            sb.Append($"[System.Serializable]\n");
            sb.Append($"public class {protoName}\n");//����
            sb.Append("{\n");
            sb.Append("\tpublic long Id;\n");

            int cellCount = sheet.GetRow(VariableNameLine).LastCellNum;

            for (int i = 1; i < cellCount; i++)
            {
                string fieldDesc = GetCellString(sheet, CommentsLine, i);
                if (fieldDesc.StartsWith("#")) continue;
                fieldDesc = fieldDesc.ToLower();

                //ID
                string fieldDes = GetCellString(sheet, CommentsLine, i);

                string fieldName = GetCellString(sheet, VariableNameLine, i);

                string fieldType = GetCellString(sheet, VariableTypeLine, i);

                if (fieldType == "" || fieldName == "") continue;

                sb.Append($"\t///{fieldDes} \n");
                sb.Append($"\tpublic {fieldType} {fieldName};\n");
            }

            sb.Append("}\n");

            sw.Write(sb.ToString());
        }
    }


    private static void Export(string filePath, string exportDir)
    {
        XSSFWorkbook xssfWorkbook;

        using FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        xssfWorkbook = new XSSFWorkbook(file);

        string protoName = Path.GetFileNameWithoutExtension(filePath);

        string exportPath = Path.Combine(exportDir, $"{protoName}.json");

        using (FileStream txt = new FileStream(exportPath, FileMode.Create))

        using (StreamWriter sw = new StreamWriter(txt))
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\n");
            sb.Append($"\"info\":[");
            sw.WriteLine(sb.ToString());
            for (int i = 0; i < xssfWorkbook.NumberOfSheets; ++i)
            {
                ISheet sheet = xssfWorkbook.GetSheetAt(i);
                ExportSheet(sheet, sw);
            }
            StringBuilder sbs = new StringBuilder();
            sbs.Append("\t]\n");
            sbs.Append("}");
            sw.WriteLine(sbs.ToString());
        }
    }

    // ����json
    private static void ExportSheet(ISheet sheet, StreamWriter sw)
    {
        //��������
        int cellCount = sheet.GetRow(VariableNameLine).LastCellNum;
        CellInfo[] cellInfos = new CellInfo[cellCount];
        for (int i = 0; i < cellCount; i++)
        {
            string fieldDesc = GetCellString(sheet, CommentsLine, i);
            string fieldName = GetCellString(sheet, VariableNameLine, i);
            string fieldType = GetCellString(sheet, VariableTypeLine, i);
            cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Description = fieldDesc };
        }

        // �ӵ����п�ʼд������itemֵ
        for (int i = DataLine; i <= sheet.LastRowNum; ++i)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\n");

            IRow row = sheet.GetRow(i);

            for (int j = 0; j < cellCount; ++j)
            {
                string desc = cellInfos[j].Description.ToLower();
                if (desc.StartsWith("#"))
                {
                    continue;
                }

                string fieldValue = GetCellString(row, j);

                if (fieldValue == "")
                {
                    //throw new Exception($"sheet: {sheet.SheetName} ���пհ��ֶ� {i},{j}");
                }

                if (j > 0)
                {
                    sb.Append(",");
                }

                string fieldName = cellInfos[j].Name;

                if (fieldName == "Id" || fieldName == "_id")
                {
                    fieldName = "Id";
                    //if (string.IsNullOrEmpty(fieldValue)) continue;
                }

                string fieldType = cellInfos[j].Type;

                if (fieldType == "int" && fieldValue == "") fieldValue = "0";

                sb.Append($"\"{fieldName}\":{Convert(fieldType, fieldValue)}");
            }
            sb.Append(i == sheet.LastRowNum ? "\n}" : "\n},");
            sw.WriteLine(sb.ToString());
        }
    }

    private static string Convert(string type, string value)
    {
        switch (type)
        {
            case "int[]":
            case "int32[]":
            case "long[]":
                return $"[{value}]";
            case "string[]":
                return $"[{value}]";
            case "int":
            case "int32":
            case "int64":
            case "long":
            case "float":
            case "double":
                return value;
            case "string":
                return $"\"{value}\"";
            default:
                throw new Exception($"��֧�ִ�����: {type}");
        }
    }

    private static string GetCellString(ISheet sheet, int i, int j)
    {
        return sheet.GetRow(i)?.GetCell(j)?.ToString() ?? "";
    }

    private static string GetCellString(IRow row, int i)
    {
        return row?.GetCell(i)?.ToString() ?? "";
    }

    private static string GetCellString(ICell cell)
    {
        return cell?.ToString() ?? "";
    }
}

