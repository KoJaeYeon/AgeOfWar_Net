using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using Unity.VisualScripting;
using System.Text;
using System.Collections.Generic;

public struct TroopData
{
    public int Gold;
    public int DieGold;
    public int Hp;
    public int Attack;

    public TroopData(int Gold, int DieGold, int Hp, int Attack)
    {
        this.Gold = Gold;
        this.DieGold = DieGold;
        this.Hp = Hp;
        this.Attack = Attack;
    }
}

public class DatabaseManager : Singleton<DatabaseManager>
{
    [Header("ConnectionInfo")]
    [SerializeField] string _ip = "127.0.0.1";
    [SerializeField] string _dbName = "test";
    [SerializeField] string _uid = "root";
    [SerializeField] string _pwd = "1234";

    private bool _isConnectTestCompolete;

    private static MySqlConnection _dbConnection;

    Dictionary<int, TroopData> troopDataDic = new Dictionary<int, TroopData>();

    public TroopData GetTroopData(int id)
    {
        return troopDataDic[id];
    }

    private void Awake()
    {
        if (Instance != this && Instance != null) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        ConnectToDB();
    }

    public void ConnectToDB()
    {
        _isConnectTestCompolete = ConnetTest();
        SendQuery();
    }

    private void SendQuery(string queryStr, string tableName)
    {
        //있으면 SELECT관련 함수 호출
        if (queryStr.Contains("SELECT"))
        {
            DataSet dataSet = OnSelectRequest(queryStr, tableName);
            string result = DeformatResult(dataSet);
        }
        //else // 없다면 INSERT 또는 UPDATE 관련 쿼리
        //{
        //    Text_DBResult.text = OnInsertOnUpdateRequest(queryStr) ? "성공" : "실패";
        //}
    }

    //public static bool OnInsertOnUpdateRequest(string query)
    //{
    //    try
    //    {
    //        MySqlCommand sqlCommand = new MySqlCommand();
    //        sqlCommand.Connection = _dbConnection;
    //        sqlCommand.CommandText = query;

    //        _dbConnection.Open();
    //        sqlCommand.ExecuteNonQuery();
    //        _dbConnection.Close();
    //        return true;
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}

    private string DeformatResult(DataSet dataSet)
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (DataTable table in dataSet.Tables)
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    stringBuilder.Append($"{row[column]},");
                }

                string tableRow = stringBuilder.ToString().TrimEnd(',');
                string[] columns = tableRow.Split(',');
                int[] columnsInt = new int[columns.Length];

                int index = 0;
                foreach(string str in columns)
                {
                    columnsInt[index++] = int.Parse(str);
                }
                troopDataDic.Add(columnsInt[0], new TroopData(columnsInt[1], columnsInt[2], columnsInt[3], columnsInt[4]));

                stringBuilder.Clear();
            }
        }
        return stringBuilder.ToString();
    }

    public static DataSet OnSelectRequest(string queryStr, string tableName)
    {
        try
        {
            _dbConnection.Open();
            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.Connection = _dbConnection;
            sqlcmd.CommandText = queryStr;

            MySqlDataAdapter sd = new MySqlDataAdapter(sqlcmd);
            DataSet dataSet = new DataSet();
            sd.Fill(dataSet, tableName);

            _dbConnection.Close();
            return dataSet;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return null;
        }
    }

    private bool ConnetTest()
    {
        string connectStr = $"Server={_ip};Database={_dbName};Uid={_uid};Pwd={_pwd};";


        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectStr))
            {
                _dbConnection = conn;
                conn.Open();
            }

            Debug.Log("DB 연결을 성공했습니다!");
            return true;
        }
        catch
        {
            Debug.Log("DB 연결을 실패했습니다!");
            return false;
        }
    }

    public void SendQuery()
    {
        if (_isConnectTestCompolete == false)
        {
            Debug.Log("DB 연결을 먼저 시도해주세요.");
            return;
        }
        string query = "SELECT * FROM War;";
        SendQuery(query, "War");

    }
}
