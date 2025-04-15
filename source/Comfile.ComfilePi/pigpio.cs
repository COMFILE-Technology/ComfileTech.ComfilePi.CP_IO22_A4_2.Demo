using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Comfile.ComfilePi
{
    internal static class pigpio
    {
        const string libraryPath = "libpigpiod_if2.so";

        public const uint PI_INPUT = 0;
        public const uint PI_OUTPUT = 1;

        public const uint PI_PUD_UP = 0;
        public const uint PI_PUD_DOWN = 1;
        public const uint PI_PUD_OFF = 2;

        [DllImport(libraryPath)]
        public static extern int pigpio_start(IntPtr addrStr, IntPtr portStr);

        [DllImport(libraryPath)]
        public static extern int pigpio_stop(int pi);

        [DllImport(libraryPath)]
        public static extern int set_mode(int pi, uint gpio, uint mode);

        [DllImport(libraryPath)]
        public static extern int gpio_write(int pi, uint gpio, uint level);

        [DllImport(libraryPath)]
        public static extern int gpio_read(int pi, uint gpio);

        [DllImport(libraryPath)]
        public static extern int set_pull_up_down(int pi, uint gpio, uint pud);

        [DllImport(libraryPath)]
        public static extern int i2c_open(int pi, uint i2c_bus, uint i2c_addr, uint i2c_flags);

        [DllImport(libraryPath)]
        public static extern int i2c_close(int pi, uint handle);

        [DllImport(libraryPath)]
        public static extern int i2c_write_byte_data(int pi, uint handle, uint i2c_reg, uint bVal);

        [DllImport(libraryPath)]
        public static extern int i2c_write_word_data(int pi, uint handle, uint i2c_reg, uint bVal);

        [DllImport(libraryPath)]
        public static extern int i2c_read_byte_data(int pi, uint handle, uint i2c_reg);

        [DllImport(libraryPath)]
        public static extern int i2c_read_word_data(int pi, uint handle, uint i2c_reg);

        [DllImport(libraryPath)]
        public unsafe static extern int i2c_read_i2c_block_data(int pi, uint handle, uint i2c_reg, byte* buf, uint count);

        [DllImport(libraryPath)]
        public unsafe static extern int i2c_write_i2c_block_data(int pi, uint handle, uint i2c_reg, byte* buf, uint count);

        [DllImport(libraryPath)]
        public unsafe static extern int i2c_read_device(int pi, uint handle, byte* buf, uint count);

        [DllImport(libraryPath)]
        public unsafe static extern int i2c_write_device(int pi, uint handle, byte* buf, uint count);
    }
}
