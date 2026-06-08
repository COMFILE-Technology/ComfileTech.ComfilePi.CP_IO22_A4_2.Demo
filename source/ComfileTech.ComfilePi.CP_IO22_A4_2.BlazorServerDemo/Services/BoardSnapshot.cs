namespace ComfileTech.ComfilePi.CP_IO22_A4_2.BlazorServerDemo.Services;

public sealed record BoardSnapshot(
    IReadOnlyList<DigitalChannelSnapshot> DigitalInputs,
    IReadOnlyList<DigitalChannelSnapshot> DigitalOutputs,
    IReadOnlyList<AnalogChannelSnapshot> AnalogInputs,
    IReadOnlyList<AnalogChannelSnapshot> AnalogOutputs,
    bool IsDisabled,
    string? ErrorMessage);

public sealed record DigitalChannelSnapshot(int Number, bool State);

public sealed record AnalogChannelSnapshot(int Channel, double Voltage);
