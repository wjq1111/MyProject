using System;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;
using UnityEditor;

//单元格信息
public struct CellInfo
{
    public string Type;//类型
    public string Name;//名称
    public string Description;//描述
}

public static class ExcelExporterEditor
{
    // Excel表格路径
    private const string ExcelPath = "./Assets/Resources/Excels/";

    // 导出的Json文件 存在Unity中的路径
    private const string clientPath = "./Assets/StreamingAssets/Config/";

    private const int CommentsLine = 0;//注释行
    private const int VariableNameLine = 1;//变量名行
    private const int VariableTypeLine = 2;//变量类型行
    private const int DataLine = 3;//数据类型行

    [MenuItem("Tools/导出Excel配置表")]
    private static void ExportConfigs()
    {
        try
        {
            //导出excel表
            ExportConfigs(clientPath);
            //导出对应的实体结构类
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
        //遍历存储表格的文件夹
        foreach (string filePath in Directory.GetFiles(ExcelPath))
        {
            //如果文件拓展名不是.xlse 说明不是表格文件 继续遍历下一个
            if (Path.GetExtension(filePath) != ".xlsx") continue;
            //如果起始是~ 说明是缓存文件 继续遍历下一个
            if (Path.GetFileName(filePath).StartsWith("~")) continue;

            string fileName = Path.GetFileName(filePath);
            Debug.Log("fileName:" + fileName);

            Export(filePath, exportDir);
        }
    }

    // 导出所有配置表为cs文件
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

    // 导出单个配置表为cs文件
    private static void ExportClass(string fileName, string exportDir, string csHead)
    {
        // 操作表格的对象
        XSSFWorkbook xssfWorkbook;
        using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            xssfWorkbook = new XSSFWorkbook(file);
        }
        // 文件名 不带后缀
        string protoName = Path.GetFileNameWithoutExtension(fileName);
        // 生产文件路径
        string exportPath = Path.Combine(exportDir, $"{protoName}.cs");
        using (FileStream txt = new FileStream(exportPath, FileMode.Create))
        using (StreamWriter sw = new StreamWriter(txt))
        {
            StringBuilder sb = new StringBuilder();
            // 获取第一张excel表
            ISheet sheet = xssfWorkbook.GetSheetAt(0);
            // 以下是要生产的格式
            sb.Append("using System.Collections.Generic;\t\n");
            sb.Append($"public class {protoName}s\n");//类名
            sb.Append("{\n");
            sb.Append($"\tpublic List<{protoName}> info;\n");
            sb.Append("}\n\n");


            sb.Append($"[System.Serializable]\n");
            sb.Append($"public class {protoName}\n");//类名
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

    // 导表json
    private static void ExportSheet(ISheet sheet, StreamWriter sw)
    {
        //变量名行
        int cellCount = sheet.GetRow(VariableNameLine).LastCellNum;
        CellInfo[] cellInfos = new CellInfo[cellCount];
        for (int i = 0; i < cellCount; i++)
        {
            string fieldDesc = GetCellString(sheet, CommentsLine, i);
            string fieldName = GetCellString(sheet, VariableNameLine, i);
            string fieldType = GetCellString(sheet, VariableTypeLine, i);
            cellInfos[i] = new CellInfo() { Name = fieldName, Type = fieldType, Description = fieldDesc };
        }

        // 从第四行开始写入所有item值
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
                    //throw new Exception($"sheet: {sheet.SheetName} 中有空白字段 {i},{j}");
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
                throw new Exception($"不支持此类型: {type}");
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

