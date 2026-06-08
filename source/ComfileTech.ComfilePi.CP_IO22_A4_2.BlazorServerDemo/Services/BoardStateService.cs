using System.Runtime.InteropServices;
using HardwareAnalogInput = ComfileTech.ComfilePi.CP_IO22_A4_2.AnalogInput;
using HardwareBoard = ComfileTech.ComfilePi.CP_IO22_A4_2.CP_IO22_A4_2;
using HardwareDigitalInput = ComfileTech.ComfilePi.CP_IO22_A4_2.DigitalInput;

namespace ComfileTech.ComfilePi.CP_IO22_A4_2.BlazorServerDemo.Services;

public sealed class BoardStateService : IDisposable
{
    private static readonly int[] DigitalInputNumbers = [4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 16];
    private static readonly int[] DigitalOutputNumbers = [17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27];
    private static readonly int[] AnalogInputChannels = [1, 2, 3, 4];
    private static readonly int[] AnalogOutputChannels = [1, 2];

    private readonly object _syncRoot = new();
    private HardwareBoard? _board;
    private BoardSnapshot _snapshot;
    private bool _disposed;

    public BoardStateService()
    {
        _snapshot = CreateDisabledSnapshot("Initializing...");

        var unsupportedHardwareMessage = GetUnsupportedHardwareMessage();
        if (unsupportedHardwareMessage is not null)
        {
            _snapshot = CreateDisabledSnapshot(unsupportedHardwareMessage);
            return;
        }

        try
        {
            _board = HardwareBoard.Instance;
            Subscribe();
            _snapshot = ReadSnapshot(isDisabled: false, errorMessage: null);
        }
        catch (Exception ex)
        {
            _board = null;
            _snapshot = CreateDisabledSnapshot(ex.Message);
        }
    }

    public event Action? StateChanged;

    public BoardSnapshot GetSnapshot()
    {
        lock (_syncRoot)
        {
            return _snapshot;
        }
    }

    public void SetDigitalOutput(int number, bool state)
    {
        Action? stateChanged = null;

        lock (_syncRoot)
        {
            if (_snapshot.IsDisabled || _board is null)
            {
                return;
            }

            var output = _board.DigitalOutputs.FirstOrDefault(o => o.Number == number);
            if (output is null)
            {
                return;
            }

            output.State = state;
            _snapshot = ReadSnapshot(isDisabled: false, errorMessage: null);
            stateChanged = StateChanged;
        }

        stateChanged?.Invoke();
    }

    public void SetAnalogOutput(int channel, double voltage)
    {
        Action? stateChanged = null;

        lock (_syncRoot)
        {
            if (_snapshot.IsDisabled || _board is null)
            {
                return;
            }

            var output = _board.AnalogOutputs.FirstOrDefault(o => o.Channel == channel);
            if (output is null)
            {
                return;
            }

            output.Voltage = Math.Clamp(voltage, 0.0, 5.0);
            _snapshot = ReadSnapshot(isDisabled: false, errorMessage: null);
            stateChanged = StateChanged;
        }

        stateChanged?.Invoke();
    }

    private void Subscribe()
    {
        if (_board is null)
        {
            return;
        }

        foreach (var input in _board.DigitalInputs)
        {
            input.StateChanged += DigitalInput_StateChanged;
        }

        foreach (var input in _board.AnalogInputs)
        {
            input.VoltageChanged += AnalogInput_VoltageChanged;
        }
    }

    private void Unsubscribe()
    {
        if (_board is null)
        {
            return;
        }

        foreach (var input in _board.DigitalInputs)
        {
            input.StateChanged -= DigitalInput_StateChanged;
        }

        foreach (var input in _board.AnalogInputs)
        {
            input.VoltageChanged -= AnalogInput_VoltageChanged;
        }
    }

    private void DigitalInput_StateChanged(HardwareDigitalInput input)
    {
        RefreshFromBoard();
    }

    private void AnalogInput_VoltageChanged(HardwareAnalogInput input)
    {
        RefreshFromBoard();
    }

    private void RefreshFromBoard()
    {
        Action? stateChanged;

        lock (_syncRoot)
        {
            if (_snapshot.IsDisabled || _board is null)
            {
                return;
            }

            _snapshot = ReadSnapshot(isDisabled: false, errorMessage: null);
            stateChanged = StateChanged;
        }

        stateChanged?.Invoke();
    }

    private BoardSnapshot ReadSnapshot(bool isDisabled, string? errorMessage)
    {
        if (_board is null)
        {
            return CreateDisabledSnapshot(errorMessage ?? "The IO board is not available.");
        }

        return new BoardSnapshot(
            _board.DigitalInputs.Select(input => new DigitalChannelSnapshot(input.Number, input.State)).ToArray(),
            _board.DigitalOutputs.Select(output => new DigitalChannelSnapshot(output.Number, output.State)).ToArray(),
            _board.AnalogInputs.Select(input => new AnalogChannelSnapshot(input.Channel, input.Voltage)).ToArray(),
            _board.AnalogOutputs.Select(output => new AnalogChannelSnapshot(output.Channel, output.Voltage)).ToArray(),
            isDisabled,
            errorMessage);
    }

    private static BoardSnapshot CreateDisabledSnapshot(string errorMessage)
    {
        return new BoardSnapshot(
            DigitalInputNumbers.Select(number => new DigitalChannelSnapshot(number, false)).ToArray(),
            DigitalOutputNumbers.Select(number => new DigitalChannelSnapshot(number, false)).ToArray(),
            AnalogInputChannels.Select(channel => new AnalogChannelSnapshot(channel, 0.0)).ToArray(),
            AnalogOutputChannels.Select(channel => new AnalogChannelSnapshot(channel, 0.0)).ToArray(),
            IsDisabled: true,
            ErrorMessage: errorMessage);
    }

    private static string? GetUnsupportedHardwareMessage()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return null;
        }

        const string modelPath = "/proc/device-tree/model";
        var model = File.Exists(modelPath) ? File.ReadAllText(modelPath).Trim('\0', '\r', '\n', ' ') : string.Empty;

        if (model.Contains("Compute Module 4S", StringComparison.OrdinalIgnoreCase) ||
            model.Contains("Compute Module 3", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return "This application should only be run on a CPi-A, CPi-B, or CPi-S series panel PC.";
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Unsubscribe();
        _board?.Dispose();
        _disposed = true;
    }
}
