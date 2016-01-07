using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using abacolla_gui;

/// <summary>
/// Summary description for BankDatabaseService
/// </summary>


/*

create table bank_phone
(
	id SERIAL not null,
	phone CHARACTER VARYING(15) not null,
	CONSTRAINT pk_bank_phone PRIMARY KEY(phone)
)

create table bank_phone_basket
(
	id SERIAL not null,
	bank_phone_id int not null,
	shop_transaction_id CHARACTER VARYING(50) not null, 
	CONSTRAINT pk_bank_phone_basket PRIMARY KEY(id)
)

create table bank_phone_page_view
(
	id SERIAL not null,
    timestamp TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT to_timestamp(0), 
	CONSTRAINT pk_bank_phone_page_view PRIMARY KEY(id)
)

*/


public class BankDatabaseService : webservice_database
{
    public bool bankPhoneExist(string sPhone)
    {
        bool bRetVal = false;

        GLOBAL_SQL_READER reader = null;
        GLOBAL_SQL_CONN conn = null;

        try
        {
            conn = new GLOBAL_SQL_CONN(this);

            GLOBAL_SQL_COMMAND command = new GLOBAL_SQL_COMMAND("select * from bank_phone where phone='" + sPhone + "' ", conn);

            reader = new GLOBAL_SQL_READER(command);

            if (reader.Read())
            {
                // string sId = reader.c("id").ToString();
                // string sBankPhone = reader.c("phone").ToString();

                bRetVal = true;
            }
        }
        catch (Exception eee)
        {
        }
        finally
        {
            reader.Close();
            conn.Close();
        }

        return bRetVal;
    }

    public bool save_phone(string sPhone)
    {
        bool bOK = false;

        try
        {
            xSQL_InsertBuilder ib = new xSQL_InsertBuilder(this, "bank_phone");
            ib.add("phone", sPhone);
            bool bOk = ib.ExecuteSql();
        }
        catch (Exception)
        {
        }
        finally
        {
        }

        return bOK;
    }


    public bool save_page_view()
    {
        bool bOK = false;

        try
        {
            xSQL_InsertBuilder ib = new xSQL_InsertBuilder(this, "bank_phone_page_view");
            ib.add("timestamp", DateTime.Now);
            bool bOk = ib.ExecuteSql();
        }
        catch (Exception)
        {
        }
        finally
        {
        }

        return bOK;
    }


    public BankDatabaseService()
    {
    }

    string sSqlException = "";
    public void setSqlException(string s)
    {
        sSqlException = s;
    }


    public string getSqlException()
    {
        return sSqlException;
    }

    public DATABASE_TYPE getDatabaseType()
    {
        return DATABASE_TYPE.POSTGRES;
    }

    public string getConnectionString()
    {
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["df_database"];
        if (settings != null)
        {
            return settings.ConnectionString;
        }
        return null;
    }



}