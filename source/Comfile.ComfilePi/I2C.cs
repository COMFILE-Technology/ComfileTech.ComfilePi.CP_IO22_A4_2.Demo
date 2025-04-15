using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comfile.ComfilePi
{
    public abstract class I2C
    {
        internal protected I2C(int pi, uint bus, byte address)
        {
            _pi = pi;
            _bus = bus;
            _address = address;
            int result = pigpio.i2c_open(_pi, _bus, _address, 0);
            if (result < 0)
            {
                throw new Exception("I2C.Open failed. " + result.ToString());
            }

            _handle = (uint)result;
        }

        ~I2C()
        {
            if (!_isDisposed)
            {
                pigpio.i2c_close(_pi, _handle);
                _isDisposed = true;
            }
        }

        bool _isDisposed = false;
        readonly int _pi;
        readonly uint _handle;
        readonly uint _bus;
        readonly byte _address;

        public uint Bus
        {
            get { return _bus; }
        }

        public byte Address
        {
            get { return _address; }
        }

        protected byte ReadByte(uint registerAddress)
        {
            int result = pigpio.i2c_read_byte_data(_pi, _handle, registerAddress);
            if (result < 0)
            {
                throw new Exception("I2C.ReadByte failed.");
            }

            return (byte)result;
        }

        protected void Write(uint registerAddres, byte value)
        {
            int result = pigpio.i2c_write_byte_data(_pi, _handle, registerAddres, value);
            if (result < 0)
            {
                throw new Exception("I2C.Write failed.");
            }
        }

        protected void Write(uint registerAddress, ushort value)
        {
            int result = pigpio.i2c_write_word_data(_pi, _handle, registerAddress, value);
            if (result < 0)
            {
                throw new Exception("I2C.Write failed.");
            }
        }

        protected void WriteBE(uint registerAddress, ushort value)
        {
            value = BitConverter.ToUInt16(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
            int result = pigpio.i2c_write_word_data(_pi, _handle, registerAddress, value);
            if (result < 0)
            {
                throw new Exception("I2C.WriteBE failed.");
            }
        }

        protected ushort ReadUint16(uint registerAddress)
        {
            int result = pigpio.i2c_read_word_data(_pi, _handle, registerAddress);
            if (result < 0)
            {
                throw new Exception("I2C.ReadUint16 failed.");
            }

            return (ushort)result;
        }

        protected short ReadInt16BE(uint registerAddress)
        {
            int result = pigpio.i2c_read_word_data(_pi, _handle, registerAddress);
            if (result < 0)
            {
                throw new Exception("I2C.ReadInt16BE failed.");
            }

            return BitConverter.ToInt16(BitConverter.GetBytes((ushort)result).Reverse().ToArray(), 0);
        }
    }
}
