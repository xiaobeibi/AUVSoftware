using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUVSoftware.Models
{
    class SFTPUtils
    {
        private SftpClient sftp;

        /// <summary>
        /// 连接状态
        /// </summary>
        private bool Connected { get { return sftp.IsConnected; } }

        /// <summary>
        /// 构造连接函数
        /// </summary>
        /// <param name="host">主机IP</param>
        /// <param name="port">端口</param>
        /// <param name="username">账户</param>
        /// <param name="password">密码</param>
        public SFTPUtils(string host, int port, string username, string password)
        {
            try
            {
                sftp = new SftpClient(host, port, username, password);
            }
            catch (Exception ex)
            {
                // 连接异常报错
                throw ex;
            }
        }

        /// <summary>
        /// 检查是否连接
        /// </summary>
        /// <returns> 连接成功返回true </returns>
        public bool Connect()
        {
            try
            {
                if (!Connected)
                {
                    sftp.Connect();
                }
                return true;
            }
            catch (Exception ex)
            {
                // 连接异常报错
                throw ex;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (sftp != null && Connected)
                {
                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                // 连接异常报错
                throw ex;
            }
        }

        /// <summary>
        /// 获取服务器中指定文件
        /// </summary>
        /// <param name="remotePath">远程路径</param>
        /// <param name="localPath">本地路径</param>
        /// <returns></returns>
        public bool Get(string remotePath, string localPath)
        {
            try
            {
                if (Connect())
                {
                    using (FileStream stream = File.Open(localPath, FileMode.OpenOrCreate))
                    {
                        sftp.DownloadFile(remotePath, stream);
                    }
                    Disconnect();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除远程服务器的文件
        /// </summary>
        /// <param name="remoteFile">远程路径</param>
        /// <returns>删除成功返回true，否则false</returns>
        public bool Delete(string remoteFile)
        {
            try
            {
                if (Connect())
                {
                    sftp.Delete(remoteFile);
                    Disconnect();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取ftp服务器上指定路径上的文件名列表
        /// </summary>
        /// <param name="remotePath">路径</param>
        /// <returns>文件名列表</returns>
        public List<string> GetFileList(string remotePath)
        {
            List<string> result = null;
            try
            {
                if (Connect())
                {
                    var files = sftp.ListDirectory(remotePath);
                    Disconnect();

                    result = new List<string>();

                    foreach (var file in files)
                    {
                        string name = file.Name;
                        result.Add(name);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
