using System;
using System.Collections.Generic;
using System.Text;


using System.Data;

using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.IO;

using System.Security.Cryptography;

    /// <summary>
    /// Denne klassen benyttes for diverse operasjoner knyttet til blob / database / og kryptering
    /// Metodene er laget slik at gjenbruk skal være mulig. 
    /// Input og output kan være både bytearray (byte[]) og fil.
    /// </summary>
public class CryptoStat
{
    // True, dersom Initialize (nødvendig pre-operasjon) har vært kalt. 
    private bool bInitialized = false;

    private string sPassword;
    private string sSaltValue;
    private int iKeySize;

    private string sHashAlgorithm = "SHA1";
    private int iPasswordIterations = 2;
    private string sInitVector = "@1B2c3D4e5F6g7H8";

    private string sConnectionString;
    public CryptoStat()
    {
    }

    // Dersom man ønsker støtte for databaseoperasjoner kan man sette en connectionstring
    public void SetConnectionString(String sConn)
    {
        sConnectionString = sConn;
    }

    // Initierer krypteringsnøkkel og saltvalue
    public bool Initialize(string _sPassword, string _sSaltValue, int _iKeySize /* 128,256 */ )
    {
        sPassword = _sPassword;
        sSaltValue = _sSaltValue;
        iKeySize = _iKeySize;

        bInitialized = true;
        return true;
    }

    public bool SaveBytesToEnctyptedSqlBlob
        (
        byte[] inBytes,
        String sSqlCommand,
        String sBlobColName
        )
    {
        SqlConnection conn = new SqlConnection(sConnectionString);

        try
        {
            if (!bInitialized) return false;

            inBytes = GetEncryptedBytes(inBytes);

            conn.Open();

            SqlCommand sqlCommand = new SqlCommand(sSqlCommand, conn);

            sqlCommand.Parameters.Add("@" + sBlobColName, SqlDbType.Image, inBytes.Length).Value = inBytes;

            sqlCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            conn.Close();
            return false;
        }
        conn.Close();
        return true;
    }

    public bool DecryptAndSaveSqlBlobToFile(
        string sFile,
        String sSqlSelect)
    {
        SqlConnection conn = new SqlConnection(sConnectionString);

        try
        {
            if (!bInitialized) return false;

            conn.Open();
            SqlDataReader sqlReader = null;

            SqlCommand sqlCommand = new SqlCommand(sSqlSelect, conn);
            sqlReader = sqlCommand.ExecuteReader();

            int bufferSize = 10000256; // Max er 10 000 000 , men vi lager et buffer ...
            byte[] outbyte = new byte[bufferSize];  // The BLOB byte[] buffer to be filled by GetBytes.

            while (sqlReader.Read())
            {
                long iBytesRead = sqlReader.GetBytes(0, 0, outbyte, 0, bufferSize);
                // Now we have the encrypted bytes
                // Decrypt !
                outbyte = DecryptBytes(outbyte, iBytesRead);
            }

            this.writeBinaryDataToFile(sFile, outbyte, outbyte.Length);
        }
        catch (Exception)
        {
            conn.Close();
            return false;
        }
        conn.Close();
        return true;
    }

    public byte[] GetEncryptedBytes(byte[] inBytes)
    {
        if (!bInitialized) return null;

        // Convert strings into byte arrays.
        // Let us assume that strings only contain ASCII codes.
        // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
        // encoding.
        byte[] initVectorBytes = Encoding.ASCII.GetBytes(sInitVector);
        byte[] saltValueBytes = Encoding.ASCII.GetBytes(sSaltValue);

        // Convert our plaintext into a byte array.
        // Let us assume that plaintext contains UTF8-encoded characters.
        // byte[] plainTextBytes  = Encoding.UTF8.GetBytes(plainText);

        // First, we must create a password, from which the key will be derived.
        // This password will be generated from the specified passphrase and 
        // salt value. The password will be created using the specified hash 
        // algorithm. Password creation can be done in several iterations.
        PasswordDeriveBytes password = new PasswordDeriveBytes(
            sPassword,
            saltValueBytes,
            sHashAlgorithm,
            iPasswordIterations);

        // Use the password to generate pseudo-random bytes for the encryption
        // key. Specify the size of the key in bytes (instead of bits).
        byte[] keyBytes = password.GetBytes(iKeySize / 8);

        // Create uninitialized Rijndael encryption object.
        RijndaelManaged symmetricKey = new RijndaelManaged();

        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC;

        // Generate encryptor from the existing key bytes and initialization 
        // vector. Key size will be defined based on the number of the key 
        // bytes.
        ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
            keyBytes,
            initVectorBytes);

        // Define memory stream which will be used to hold encrypted data.
        MemoryStream memoryStream = new MemoryStream();

        // Define cryptographic stream (always use Write mode for encryption).
        CryptoStream cryptoStream = new CryptoStream(memoryStream,
            encryptor,
            CryptoStreamMode.Write);
        // Start encrypting.
        cryptoStream.Write(inBytes, 0, inBytes.Length);

        // Finish encrypting.
        cryptoStream.FlushFinalBlock();

        // Convert our encrypted data from a memory stream into a byte array.
        byte[] cipherTextBytes = memoryStream.ToArray();

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert encrypted data into a base64-encoded string.
        return cipherTextBytes;
    }

    public bool EncryptFile(string sInFile, string sOutFile)
    {
        byte[] encryptedBytes = EncryptFromFile(sInFile, sPassword, sSaltValue, sHashAlgorithm, iPasswordIterations, sInitVector, iKeySize);
        writeBinaryDataToFile(sOutFile, encryptedBytes, encryptedBytes.Length);
        return true;
    }

    private byte[] EncryptFromFile(
        string sFile,
        string passPhrase,
        string saltValue,
        string hashAlgorithm,
        int passwordIterations,
        string initVector,
        int keySize)
    {

        String plainText = getTextFromFile(sFile);

        int documentNumberOfBytes = getBinaryDataFileLength(sFile);

        byte[] plainTextBytes = getBinaryDataFromFile(sFile, documentNumberOfBytes);


        //			int documentNumberOfBytes = getBinaryDataFromFile(sFile,document);
        //			byte[] plainTextBytes = document;
        //			FileLib.writeBinaryDataToFile(sOutFile,document,length);

        // Convert strings into byte arrays.
        // Let us assume that strings only contain ASCII codes.
        // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
        // encoding.
        byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
        byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

        // Convert our plaintext into a byte array.
        // Let us assume that plaintext contains UTF8-encoded characters.
        // byte[] plainTextBytes  = Encoding.UTF8.GetBytes(plainText);

        // First, we must create a password, from which the key will be derived.
        // This password will be generated from the specified passphrase and 
        // salt value. The password will be created using the specified hash 
        // algorithm. Password creation can be done in several iterations.
        PasswordDeriveBytes password = new PasswordDeriveBytes(
            passPhrase,
            saltValueBytes,
            hashAlgorithm,
            passwordIterations);

        // Use the password to generate pseudo-random bytes for the encryption
        // key. Specify the size of the key in bytes (instead of bits).
        byte[] keyBytes = password.GetBytes(keySize / 8);

        // Create uninitialized Rijndael encryption object.
        RijndaelManaged symmetricKey = new RijndaelManaged();

        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC;

        // Generate encryptor from the existing key bytes and initialization 
        // vector. Key size will be defined based on the number of the key 
        // bytes.
        ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
            keyBytes,
            initVectorBytes);

        // Define memory stream which will be used to hold encrypted data.
        MemoryStream memoryStream = new MemoryStream();

        // Define cryptographic stream (always use Write mode for encryption).
        CryptoStream cryptoStream = new CryptoStream(memoryStream,
            encryptor,
            CryptoStreamMode.Write);
        // Start encrypting.
        cryptoStream.Write(plainTextBytes, 0, documentNumberOfBytes);

        // Finish encrypting.
        cryptoStream.FlushFinalBlock();

        // Convert our encrypted data from a memory stream into a byte array.
        byte[] cipherTextBytes = memoryStream.ToArray();

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert encrypted data into a base64-encoded string.
        return cipherTextBytes;
    }

    private String getTextFromFile(String sFile)
    {
        StreamReader sr = File.OpenText(sFile);
        String buffer = "";

        String s = sr.ReadLine();

        while (s != null)
        {
            buffer += s;
            s = sr.ReadLine();
        }
        sr.Close();
        return buffer;
    }

    private int getBinaryDataFileLength(String sFile)
    {
        FileStream fs = File.OpenRead(sFile);
        BinaryReader br = new BinaryReader(fs);
        byte[] bytes = br.ReadBytes(1);
        int bytesRead = 0;
        while (bytes.Length == 1)
        {
            ++bytesRead;
            bytes = br.ReadBytes(1);
        }
        br.Close();
        return bytesRead;
    }

    public byte[] getBytesFromFile(String sFile)
    {
        int binaryLength = getBinaryDataFileLength(sFile);
        return getBinaryDataFromFile(sFile, binaryLength);
    }

    private byte[] getBinaryDataFromFile(String sFile, int iLength)
    {
        byte[] document = new byte[iLength];
        FileStream fs = File.OpenRead(sFile);
        BinaryReader br = new BinaryReader(fs);
        byte[] bytes = br.ReadBytes(1);
        int bytesRead = 0;
        while (bytes.Length == 1)
        {
            document[bytesRead] = bytes[0];
            ++bytesRead;
            bytes = br.ReadBytes(1);
        }
        br.Close();
        return document;
    }


    private bool writeBinaryDataToFile(String sOutFile, byte[] document, long length)
    {
        FileStream fs = File.Create(sOutFile);
        BinaryWriter bw = new BinaryWriter(fs);
        bw.Write(document, 0, (int)length);
        bw.Close();
        return true;
    }

    public bool DecryptFile(string sFile, string sOutFile)
    {
        int binaryLength = getBinaryDataFileLength(sFile);

        byte[] cipherTextBytes = getBinaryDataFromFile(sFile, binaryLength);

        // Convert strings defining encryption key characteristics into byte
        // arrays. Let us assume that strings only contain ASCII codes.
        // If strings include Unicode characters, use Unicode, UTF7, or UTF8
        // encoding.
        byte[] initVectorBytes = Encoding.ASCII.GetBytes(sInitVector);
        byte[] saltValueBytes = Encoding.ASCII.GetBytes(sSaltValue);

        // Convert our ciphertext into a byte array.
        // byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

        // First, we must create a password, from which the key will be 
        // derived. This password will be generated from the specified 
        // passphrase and salt value. The password will be created using
        // the specified hash algorithm. Password creation can be done in
        // several iterations.
        PasswordDeriveBytes password = new PasswordDeriveBytes(
            sPassword,
            saltValueBytes,
            sHashAlgorithm,
            iPasswordIterations);

        // Use the password to generate pseudo-random bytes for the encryption
        // key. Specify the size of the key in bytes (instead of bits).
        byte[] keyBytes = password.GetBytes(iKeySize / 8);

        // Create uninitialized Rijndael encryption object.
        RijndaelManaged symmetricKey = new RijndaelManaged();

        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC;

        // Generate decryptor from the existing key bytes and initialization 
        // vector. Key size will be defined based on the number of the key 
        // bytes.
        ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
            keyBytes,
            initVectorBytes);

        // Define memory stream which will be used to hold encrypted data.
        MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

        // Define cryptographic stream (always use Read mode for encryption).
        CryptoStream cryptoStream = new CryptoStream(memoryStream,
            decryptor,
            CryptoStreamMode.Read);

        // Since at this point we don't know what the size of decrypted data
        // will be, allocate the buffer long enough to hold ciphertext;
        // plaintext is never longer than ciphertext.
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        // Start decrypting.
        int decryptedByteCount = cryptoStream.Read(plainTextBytes,
            0,
            plainTextBytes.Length);

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert decrypted data into a string. 
        // Let us assume that the original plaintext string was UTF8-encoded.

        writeBinaryDataToFile(sOutFile, plainTextBytes, decryptedByteCount);

        return true;
    }

    public byte[] DecryptBytes(byte[] inBytes, long iBufferSize)
    {

        byte[] cipherTextBytes = new byte[iBufferSize];

        for (int i = 0; i < iBufferSize; ++i)
            cipherTextBytes[i] = inBytes[i];

        // Convert strings defining encryption key characteristics into byte
        // arrays. Let us assume that strings only contain ASCII codes.
        // If strings include Unicode characters, use Unicode, UTF7, or UTF8
        // encoding.
        byte[] initVectorBytes = Encoding.ASCII.GetBytes(sInitVector);
        byte[] saltValueBytes = Encoding.ASCII.GetBytes(sSaltValue);

        // Convert our ciphertext into a byte array.
        // byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

        // First, we must create a password, from which the key will be 
        // derived. This password will be generated from the specified 
        // passphrase and salt value. The password will be created using
        // the specified hash algorithm. Password creation can be done in
        // several iterations.
        PasswordDeriveBytes password = new PasswordDeriveBytes(
            sPassword,
            saltValueBytes,
            sHashAlgorithm,
            iPasswordIterations);

        // Use the password to generate pseudo-random bytes for the encryption
        // key. Specify the size of the key in bytes (instead of bits).
        byte[] keyBytes = password.GetBytes(iKeySize / 8);

        // Create uninitialized Rijndael encryption object.
        RijndaelManaged symmetricKey = new RijndaelManaged();

        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC;

        // Generate decryptor from the existing key bytes and initialization 
        // vector. Key size will be defined based on the number of the key 
        // bytes.
        ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
            keyBytes,
            initVectorBytes);

        // Define memory stream which will be used to hold encrypted data.
        MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

        // Define cryptographic stream (always use Read mode for encryption).
        CryptoStream cryptoStream = new CryptoStream(memoryStream,
            decryptor,
            CryptoStreamMode.Read);

        // Since at this point we don't know what the size of decrypted data
        // will be, allocate the buffer long enough to hold ciphertext;
        // plaintext is never longer than ciphertext.
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        // Start decrypting.
        int decryptedByteCount = cryptoStream.Read(plainTextBytes,
            0,
            plainTextBytes.Length);

        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();

        // Convert decrypted data into a string. 
        // Let us assume that the original plaintext string was UTF8-encoded.

        byte[] returnBytes = new byte[decryptedByteCount];

        for (int i = 0; i < decryptedByteCount; ++i)
            returnBytes[i] = plainTextBytes[i];

        return returnBytes;
    }


    public bool SaveBytesToEnctyptedSqlBlob
        (
        String sStoredProcedure,
        String sIDCol,
        object idValue,
        String sBlobCol,
        byte[] blobBytes
        )
    {

        SqlConnection conn = new SqlConnection(sConnectionString);

        try
        {
            blobBytes = GetEncryptedBytes(blobBytes);

            conn.Open();

            SqlCommand cmdStored = new SqlCommand(sStoredProcedure, conn);
            cmdStored.CommandType = CommandType.StoredProcedure;

            cmdStored.Parameters.Add("@" + sBlobCol, SqlDbType.Image);
            cmdStored.Parameters["@" + sBlobCol].Value = blobBytes;

            if (idValue.GetType() == Type.GetType("System.Int32") || idValue.GetType() == Type.GetType("System.Int16"))
            {
                cmdStored.Parameters.Add("@" + sIDCol, SqlDbType.Int);
                cmdStored.Parameters["@" + sIDCol].Value = (int)idValue;
            }
            else if (idValue.GetType() == Type.GetType("System.String"))
            {
                cmdStored.Parameters.Add("@" + sIDCol, SqlDbType.Text);
                cmdStored.Parameters["@" + sIDCol].Value = (string)idValue;
            }
            else
                return false;

            cmdStored.ExecuteNonQuery();
        }
        catch (Exception)
        {
            conn.Close();
            return false;
        }
        conn.Close();
        return true;
    }
}

