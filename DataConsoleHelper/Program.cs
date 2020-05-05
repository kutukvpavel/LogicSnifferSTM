using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;

namespace DataConsoleHelper
{
    struct Range<T> where T : IComparable<T>
    {
        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }           

        public T Min;
        public T Max;

        public int Length
        {
            get
            {
                try
                {
                    int i = Convert.ToInt32(Min);
                    int a = Convert.ToInt32(Max);
                    return Math.Abs(a - i);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Range Length exception: " + e.Message);
                    return -1;         
                }
            }
        }

        public bool Contains(T val)
        {
            return (val.CompareTo(Min) >= 0) && (val.CompareTo(Max) <= 0);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return (Range<T>)obj == this;
        }
        public override int GetHashCode()
        {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }

        public static bool operator ==(Range<T> r, Range<T> t)
        {
            return (r.Max.CompareTo(t.Max) == 0) && (r.Min.CompareTo(t.Min) == 0);
        }
        public static bool operator !=(Range<T> r, Range<T> t)
        {
            return (r.Max.CompareTo(t.Max) != 0) || (r.Min.CompareTo(t.Min) != 0);
        }
        public static bool operator <(Range<T> r, Range<T> t)
        {
            int c1 = t.Min.CompareTo(r.Min);
            int c2 = t.Max.CompareTo(r.Max);
            return ((c1 < 0) && (c2 > 0)) || ((c1 == 0) && (c2 > 0)) || ((c1 < 0) && (c2 == 0));
        }
        public static bool operator >(Range<T> r, Range<T> t)
        {
            int c1 = r.Min.CompareTo(t.Min); 
            int c2 = r.Max.CompareTo(t.Max);
            return ((c1 < 0) && (c2 > 0)) || ((c1 == 0) && (c2 > 0)) || ((c1 < 0) && (c2 == 0));
        }
        public static bool operator >=(Range<T> r, Range<T> t)
        {
            int c1 = r.Min.CompareTo(t.Min);
            int c2 = r.Max.CompareTo(t.Max);
            return ((c1 < 0) && (c2 > 0)) || ((c1 == 0) && (c2 > 0)) || ((c1 < 0) && (c2 == 0)) || ((c1 == 0) && (c2 == 0));
        }
        public static bool operator <=(Range<T> r, Range<T> t)
        {
            int c1 = t.Min.CompareTo(r.Min);
            int c2 = t.Max.CompareTo(r.Max);
            return ((c1 < 0) && (c2 > 0)) || ((c1 == 0) && (c2 > 0)) || ((c1 < 0) && (c2 == 0)) || ((c1 == 0) && (c2 == 0));
        }
    }

    class Program
    {
        public class OneWire
        {
            public class Pulse    //For protocol capture analysis
            {
                //Static
                public enum OperationClass
                {
                    OWNoClass,
                    OWUtilitarian,
                    OWReadWrite,
                    OWAnyClass
                }
                public enum PulseTypes : uint
                {
                    OWReset,
                    OWPresence,
                    OWWriteOne,
                    OWWriteZero,
                    OWErrorType,
                    OWAnyType
                }
                private static readonly Range<uint>[] types_durations = {
                    new Range<uint>(450, 600),
                    new Range<uint>(60, 240),
                    new Range<uint>(1, 15),
                    new Range<uint>(25, 120),
                    new Range<uint>(0, 0),  //Error
                    new Range<uint>(0, 600) //Any type container
                };
                private static readonly string[] types_names = { "Reset", "Presence", "WriteOne", "WriteZero", "Error-Type", "Any Type" };
                private static readonly string[] operation_names = { "No Class", "Utility", "Read/Write", "Any Class" };
                private static readonly string[] types_names_numeric = { "Reset", "Presence", "1", "0", "", "" };
                private static readonly string[] operation_names_short = { "", "Utility", "RW", "" };
                public static Range<uint> GetTypeDuration(PulseTypes type)
                {
                    return types_durations[(uint)type];
                }
                public static string GetTypeName(PulseTypes type, bool shorten = false)
                {
                    return shorten ? types_names_numeric[(uint)type] : types_names[(uint)type];
                }
                public static string GetOperationClassName(OperationClass cls, bool shorten = false)
                {
                    return shorten ? operation_names_short[(uint)cls] : operation_names[(uint)cls];
                }
                public static PulseTypes[] DurationToType(uint duration)
                {
                    List<PulseTypes> possible = new List<PulseTypes>(types_durations.Length);
                    for (uint i = 0; i < types_durations.Length; i++)
                    {
                        if (types_durations[i].Contains(duration)) possible.Add((PulseTypes)i);
                    }
                    if (possible.Count == 0)
                    {
                        return new PulseTypes[] { PulseTypes.OWErrorType };
                    }   
                    return possible.ToArray();
                }
                public static OperationClass TypeToOpClass(PulseTypes type)
                {
                    switch (type)
                    {
                        case PulseTypes.OWReset:
                            return OperationClass.OWUtilitarian;    
                        case PulseTypes.OWPresence:
                            return OperationClass.OWUtilitarian;   
                        case PulseTypes.OWWriteOne:
                            return OperationClass.OWReadWrite;   
                        case PulseTypes.OWWriteZero:
                            return OperationClass.OWReadWrite;
                        case PulseTypes.OWAnyType:
                            return OperationClass.OWAnyClass;   
                        default:
                            return OperationClass.OWNoClass;
                    }
                }

                //Non-static
                public Pulse(uint time, uint duration)
                {
                    Time = time;
                    Duration = duration;   
                }                             
                public bool ChooseType(uint possible_index)
                {
                    if (possible_index >= _possible_types.Length) return false;
                    _type = _possible_types[possible_index];
                    _op_class = _possible_opclass[possible_index];
                    return true;
                }
                public bool ChooseType(PulseTypes right_type)
                {
                    for (uint i = 0; i < _possible_types.Length; i++)
                    {
                        if (_possible_types[i] == right_type)
                        {
                            return ChooseType(i);
                        }
                    }
                    return false;
                }

                public uint Time;

                private PulseTypes _type;
                private PulseTypes[] _possible_types;
                private OperationClass _op_class = OperationClass.OWNoClass;
                private OperationClass[] _possible_opclass = new OperationClass[] { OperationClass.OWNoClass }; 
                public OperationClass OpClass
                {
                    get { return _op_class; }
                }
                public OperationClass[] PossibleOpClasses
                {
                    get { return _possible_opclass; }
                }

                private uint _duration;
                public uint Duration
                {
                    get { return _duration; }
                    set
                    {
                        _duration = value;
                        _possible_types = DurationToType(_duration);
                        _type = _possible_types[0];
                        for (uint i = 1; i < _possible_types.Length; i++)        //Choose narrower type
                        {
                            if (types_durations[(uint)_possible_types[i]]/*.Length*/ <= types_durations[(uint)_type]/*.Length*/)
                            {
                                _type = _possible_types[i];
                            }
                        }
                        if (_type != PulseTypes.OWErrorType)
                        { 
                            _op_class = TypeToOpClass(_type);
                            _possible_opclass = new OperationClass[_possible_types.Length];
                            for (uint i = 0; i < _possible_types.Length; i++)
                            {
                                _possible_opclass[i] = TypeToOpClass(_possible_types[i]);
                            }
                        }
                    }
                }
                public PulseTypes Type
                {
                    get { return _type; }
                }
                public Range<uint> TypeDuration
                {
                    get { return types_durations[(uint)_type]; }
                }
                public PulseTypes[] PossibleTypes
                {
                    get { return _possible_types; }
                }
                public string TypeName
                {
                    get { return UseShortNames ? types_names_numeric[(uint)_type] : types_names[(uint)_type]; }
                }
                public string OpClassName
                {
                    get { return UseShortNames ? operation_names_short[(uint)_op_class] : operation_names[(uint)_op_class]; }
                }
                public bool RequiresCorrection
                {
                    get { return _possible_types.Length > 1; }
                }
                public bool UseShortNames { get; set; }
            }

            public class Slave
            {
                public Slave(ref CommandRow<Pulse.OperationClass> op_class, ref CommandRow<Pulse.PulseTypes> p_types,
                    DecodeFunction func, string name = "N/A")
                {
                    _name = name;
                    _op_class = op_class;
                    _p_types = p_types;
                    _func = func;
                }

                private string _name;
                private CommandRow<Pulse.OperationClass> _op_class;
                private CommandRow<Pulse.PulseTypes> _p_types;
                private DecodeFunction _func;
                                            
                public delegate ulong DecodeFunction(ref List<Pulse> pulses, uint start);
                public string Name
                {
                    get { return _name; }
                }
                public CommandRow<Pulse.OperationClass> SlaveOperationStructure
                {
                    get { return _op_class; }
                }
                public CommandRow<Pulse.PulseTypes> SlaveCommandPattern
                {
                    get { return _p_types; }
                }
                public ulong DecodeData(ref List<Pulse> pulses, uint start)
                {
                    return _func(ref pulses, start);
                }
            }

            public class CommandRow<T> where T : IComparable
            {
                public CommandRow()    
                {
                    Constructor(new KeyValuePair<T, uint>[] { });
                }
                public CommandRow(T arg, uint number_of_identical)
                {
                    Constructor(new KeyValuePair<T, uint>[]
                    {
                        new KeyValuePair<T, uint>(arg, number_of_identical)
                    });
                }
                public CommandRow(KeyValuePair<T, uint>[] row)
                {
                    Constructor(row);
                }
                private void Constructor(KeyValuePair<T, uint>[] row)
                {
                    _row = row.ToList();          
                    _arr = new List<T>();    
                    for (int i = 0; i < _row.Count; i++)
                    {
                        for (uint j = 0; j < _row[i].Value; j++)
                        {
                            _arr.Add(_row[i].Key);
                        }                    
                    }
                }

                private List<T> _arr;
                private List<KeyValuePair<T, uint>> _row;

                public void Add(T commad, uint len)
                {
                    _row.Add(new KeyValuePair<T, uint>(commad, len));
                    for (int i = 0; i < len; i++)
                    {
                        _arr.Add(commad);
                    }
                }
                public bool Conforms(ref List<Pulse> pulses, uint start, uint stop)
                {     
                    bool cmp;
                    if (typeof(T) == typeof(Pulse.OperationClass))
                    {
                        cmp = true;
                    }
                    else
                    {
                        if (typeof(T) == typeof(Pulse.PulseTypes))
                        {
                            cmp = false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (_arr.Count > (stop - start))
                    {
                        return false;
                    }
                    for (int i = 0; i < _arr.Count; i++)
                    {
                        if ((cmp ? _arr[i].CompareTo(Pulse.OperationClass.OWAnyClass) : _arr[i].CompareTo(Pulse.PulseTypes.OWAnyType)) == 0)
                        {
                            continue;
                        }
                        if ((cmp ? _arr[i].CompareTo(pulses[(int)(i + start)].OpClass) : _arr[i].CompareTo(pulses[(int)(i + start)].Type)) != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                public int FisrtIndexOf(T element)
                {
                    for (int i = 0; i < _arr.Count; i++)
                    {
                        if (_arr[i].CompareTo(element) == 0)
                        {
                            return i;
                        }
                    }
                    return -1;
                }
                public bool Contains(T element)
                {
                    return _arr.Contains(element);
                }
                public T[] ActualRow
                {
                    get { return _arr.ToArray(); }
                }
                public KeyValuePair<T, uint>[] InitialRow
                {
                    get { return _row.ToArray(); }
                }
                public uint Length
                {
                    get { return (uint)_arr.Count; }
                }      
                public T this[uint index]
                {
                    get { return _arr[(int)index]; }
                }
            }

            public class Transaction
            {
                private List<T> CopyList<T>(ref List<T> orig, uint start, uint end)
                {
                    uint len = end - start;
                    List<T> res = new List<T>((int)len);
                    for (uint i = 0; i < len; i++)
                    {
                        res.Add(orig[(int)(i + start)]);
                    }
                    return res;
                }
                private CommandRow<Errors> ScanInnerErrors()
                {
                    if (_pulses.Count == 0) return null;
                    CommandRow<Errors> res = new CommandRow<Errors>();
                    uint j = 1;
                    Errors last_error = Errors.TRNoErorrs;
                    //Pulse.OperationClass last_class = _pulses[0].OpClass;
                    if (_pulses[0].Type == Pulse.PulseTypes.OWErrorType)
                    {       
                        last_error = Errors.OWTypeError;
                    }
                    for (int i = 1; i < _pulses.Count; i++)
                    {
                        switch (_pulses[i].OpClass)
                        {
                            case Pulse.OperationClass.OWNoClass:
                                if (_pulses[i].Type == Pulse.PulseTypes.OWErrorType)
                                {
                                    if (last_error == Errors.OWTypeError)
                                    {
                                        j++;
                                    }
                                    else
                                    {
                                        res.Add(last_error, j);
                                        last_error = Errors.OWTypeError;
                                        j = 1;
                                    }
                                }
                                else
                                {
                                    goto default;
                                }
                                break;
                            case Pulse.OperationClass.OWReadWrite: //Check timeslot
                                if (_pulses[i - 1].OpClass == Pulse.OperationClass.OWReadWrite)
                                {
                                    if (TimeslotViolation(_pulses[i - 1], _pulses[i]))
                                    {
                                        if (last_error == Errors.OWTimeslotViolation)
                                        {
                                            j++;
                                        }
                                        else
                                        {
                                            res.Add(last_error, j);
                                            last_error = Errors.OWTimeslotViolation;
                                            j = 1;
                                        }
                                    }
                                    else
                                    {
                                        goto default;
                                    }
                                }
                                else
                                {
                                    goto default;
                                }
                                break;
                            default:
                                if (last_error == Errors.TRNoErorrs)
                                {
                                    j++;
                                }
                                else
                                {
                                    res.Add(last_error, j);
                                    last_error = Errors.TRNoErorrs;
                                    j = 1;
                                }
                                break;
                        }
                    }
                    res.Add(last_error, j);
                    return res;
                }

                public Transaction(ref List<Pulse> pulses, uint master, Errors error, uint start, uint end)    //Slave error
                {
                    _pulses = CopyList(ref pulses, start, end);
                    _error = error;
                    _master = master;
                    _inner_error = ScanInnerErrors();
                    _tr_index = start;
                }
                public Transaction(ref List<Pulse> pulses, Errors error, uint start, uint end)   //Master error
                {
                    _pulses = CopyList(ref pulses, start, end);
                    if (error == Errors.TRMasterError)
                    {
                        if (DetectNoPresenceError())
                        {
                            _error = Errors.TRNoPresence;
                        }
                        else if (_pulses.Count == MasterCommandPattern.Length)
                        {
                            _error = Errors.TRNoCommand;
                        }
                    }
                    else
                    {   
                        _error = error;
                    }
                    _inner_error = ScanInnerErrors();
                    _tr_index = start;
                }
                public Transaction(ref List<Pulse> pulses, uint master, ulong slave, uint start, uint end)  //No error
                {
                    _pulses = CopyList(ref pulses, start, end);
                    _master = master;
                    _slave = slave;
                    _tr_index = start;
                    _inner_error = ScanInnerErrors();
                    if (_inner_error.Contains(Errors.OWTimeslotViolation)) //This error is detected only inside a transaction
                    {
                        _error = Errors.OWTimeslotViolation;
                    }
                }

                public static readonly uint TimeslotMin = 59;
                public static bool TimeslotViolation(Pulse first, Pulse second)
                {
                    bool res = Math.Abs(second.Time - first.Time) < TimeslotMin;
                    return res;
                }

                public enum Errors : uint
                {
                    TRNoErorrs,
                    TRNoPresence,
                    TRMasterError,
                    TRSlaveError,
                    OWTypeError,
                    OWTimeslotViolation,
                    TRNoCommand
                }
                private static readonly string[] err_names =
                {
                    "No Error", "No Presence", "Master Error", "Slave Error", "Type Error", "Timeslot Violation", "No Command"
                };                                                                        

                private List<Pulse> _pulses;
                private uint _master;
                private ulong _slave;
                private Errors _error = Errors.TRNoErorrs;     //Is set up by instance invoker (based on Conforms)
                private CommandRow<Errors> _inner_error = new CommandRow<Errors>();   //Is set up automatically
                private uint _tr_index;

                private bool DetectNoPresenceError()
                {         
                    for (int i = 0; i < _pulses.Count; i++)
                    {
                        if (_pulses[i].Type == Pulse.PulseTypes.OWPresence)
                        {
                            return false;
                        }
                    }
                    return true;
                }

                public static string ErrorName(Errors err)
                {
                    return err_names[(uint)err];
                }
                public Errors Error
                {
                    get { return _error; }
                }
                public ulong SlaveData
                {
                    get { return _slave; }
                } 
                public uint MasterCommand
                {
                    get { return _master; }
                }
                public Pulse.PulseTypes this[int index]
                {
                    get { return _pulses[index].Type; }
                }
                public string HEXSlave
                {
                    get { return _slave.ToString("X"); }
                }
                public string HEXMaster
                {
                    get { return _master.ToString("X"); }
                }
                public CommandRow<Errors> InnerErrors
                {
                    get { return _inner_error; }
                }
                public Pulse[] Pulses
                {
                    get { return _pulses.ToArray(); }
                }
                public uint Index
                {
                    get { return _tr_index; }
                }
                public KeyValuePair<bool, bool> DataParsable
                {
                    get
                    {
                        switch (_error)
                        {
                            case Errors.TRNoErorrs:
                                return new KeyValuePair<bool, bool>(true, true);
                            case Errors.TRNoPresence:
                                return new KeyValuePair<bool, bool>(false, false);
                            case Errors.TRMasterError:
                                return new KeyValuePair<bool, bool>(false, false);
                            case Errors.TRSlaveError:
                                return new KeyValuePair<bool, bool>(true, false);
                            case Errors.OWTypeError:
                                return new KeyValuePair<bool, bool>(true, true);
                            case Errors.OWTimeslotViolation:
                                return new KeyValuePair<bool, bool>(true, true);
                            case Errors.TRNoCommand:
                                return new KeyValuePair<bool, bool>(false, false);
                            default:
                                return new KeyValuePair<bool, bool>(true, true);
                        }
                    }
                }
            }

            //Constructors                            
            public OneWire(ref List<Pulse> pulses, Slave slave_description)
            {
                _slave = slave_description;
                _pulses = pulses;
                Compute();
            }

            //Private

            private List<Transaction> _trans = new List<Transaction>();
            private List<Pulse> _pulses;
            private Slave _slave;       

            //Constants
            //Brief structure, only operation type (all required)                                                      
            public static readonly CommandRow<Pulse.OperationClass> MasterOperationStructure = new CommandRow<Pulse.OperationClass>(
                new KeyValuePair<Pulse.OperationClass, uint>[] {
                new KeyValuePair<Pulse.OperationClass, uint>(Pulse.OperationClass.OWUtilitarian, 2),
                new KeyValuePair<Pulse.OperationClass, uint>(Pulse.OperationClass.OWReadWrite, 8)
            });
            //Detailed commands (are checked if specified)                                                        
            public static readonly CommandRow<Pulse.PulseTypes> MasterCommandPattern = new CommandRow<Pulse.PulseTypes>(
                new KeyValuePair<Pulse.PulseTypes, uint>[] {
                new KeyValuePair<Pulse.PulseTypes, uint>(Pulse.PulseTypes.OWReset, 1),
                new KeyValuePair<Pulse.PulseTypes, uint>(Pulse.PulseTypes.OWPresence, 1)
            });

            //Methods                                      
            private void Compute()
            {
                List<uint> reset = new List<uint>(_pulses.Count / 2);
                for (uint i = 0; i < (uint)_pulses.Count; i++)        //Find all resets
                {
                    if (_pulses[(int)i].Type == Pulse.PulseTypes.OWReset)
                    {
                        reset.Add(i);
                    }
                }
                try  //Heuristics are not so exception-proof
                {
                    List<int>[] corrections = CorrectTypeDetection();
                    Console.WriteLine("Successful corrections at following pulses:");
                    foreach (var item in corrections[1])
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("EOF" + Environment.NewLine + "Failed corrections at following pulses:");
                    foreach (var item in corrections[0])
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("EOF");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught during type correction: " + e.Message);       
                }
                reset.Add((uint)_pulses.Count); //Last transaction has no following reset
                for (int i = 1; i < reset.Count; i++)   //Parse all packets one-by-one
                {
                    if (MasterOperationStructure.Conforms(ref _pulses, reset[i - 1], reset[i]) &&      //If master command structure recognized
                        MasterCommandPattern.Conforms(ref _pulses, reset[i - 1], reset[i]))
                    {   
                        if (_slave.SlaveOperationStructure.Conforms(ref _pulses, reset[i - 1] + MasterOperationStructure.Length, reset[i]) &&
                            _slave.SlaveCommandPattern.Conforms(ref _pulses, reset[i - 1] + MasterOperationStructure.Length, reset[i]))  //If slave structure recognized
                        {    //Parse slave's response
                            _trans.Add(new Transaction(ref _pulses, ParseMasterCommand(ref _pulses, reset[i - 1] + MasterCommandPattern.Length),
                                _slave.DecodeData(ref _pulses, reset[i - 1] + MasterOperationStructure.Length), reset[i - 1], reset[i]));
                        }
                        else
                        {
                            _trans.Add(new Transaction(ref _pulses, ParseMasterCommand(ref _pulses, reset[i - 1] + MasterCommandPattern.Length),
                                Transaction.Errors.TRSlaveError, reset[i - 1], reset[i]));
                        }
                    }
                    else
                    {
                        _trans.Add(new Transaction(ref _pulses, Transaction.Errors.TRMasterError, reset[i - 1], reset[i]));
                    } 
                }
            }

            private List<int>[] CorrectTypeDetection()    //This stuff has to be transaction-unbound because it has to run before any of them are found
            {                                             //Therefore we have to implement legacy transaction logic here
                if (MasterCommandPattern.Length == 0) return null;
                List<int> master_seq_starts = new List<int>(_pulses.Count / 2);      
                uint len = MasterOperationStructure.Length + _slave.SlaveOperationStructure.Length;   //Expected transaction length
                master_seq_starts.Add(_pulses.IndexOf(_pulses.Where(x => x.Type == MasterCommandPattern[0]).First()));
                for (int i = master_seq_starts.First() + 1; i < _pulses.Count; i++) //Find all transaction entry points
                {
                    if (_pulses[i].Type == MasterCommandPattern[0])
                    {
                        if (i - master_seq_starts.Last() >= len)        //Aborted transactions will not be corrected
                        {
                            master_seq_starts.Add(i);
                        }
                    }     
                }
                List<int>[] res = new List<int>[]
                {
                    new List<int>((int)(len * master_seq_starts.Count)),
                    new List<int>((int)(len * master_seq_starts.Count))
                };
                for (int i = 0; i < master_seq_starts.Count; i++)
                {
                    for (int j = 0; j < len; j++)
                    {                                       
                        int current = master_seq_starts[i] + j;
                        if (current >= _pulses.Count) break;
                        if (_pulses[current].RequiresCorrection)
                        {
                            Pulse.PulseTypes corr = Pulse.PulseTypes.OWErrorType;    //Expected type
                            if (j < MasterOperationStructure.Length)  //Use master's command pattern
                            {
                                if (j < MasterCommandPattern.Length)   //If we have exact commands
                                {
                                    corr = MasterCommandPattern[(uint)j];
                                }
                                else     //If we have to deduce commands based on pulse duration and operation class
                                {
                                    corr = correction_helper(MasterOperationStructure[(uint)j], _pulses[current]);
                                }
                            }
                            else   //Use slave's command pattern
                            {
                                if ((j - MasterOperationStructure.Length) < _slave.SlaveCommandPattern.Length)   //If we have exact commands
                                {
                                    corr = _slave.SlaveCommandPattern[(uint)(j - MasterOperationStructure.Length)];
                                }
                                else     //If we have to deduce commands based on pulse duration and operation class
                                {
                                    corr = correction_helper(_slave.SlaveOperationStructure[(uint)(j - MasterOperationStructure.Length)], _pulses[current]);
                                }
                            }
                            if (_pulses[current].Type != corr)     //If we actually have to correct the type chosen automatically
                            {
                                if (_pulses[current].PossibleTypes.Contains(corr))  //If such correction is valid (helper can return OWErrorType)
                                {
                                    res[Convert.ToInt32(_pulses[current].ChooseType(corr))].Add(current);
                                }
                                else
                                {
                                    res[1].Add(current);
                                }                                       
                            }
                        }
                    }
                }
                return res;
            }
            private Pulse.PulseTypes correction_helper(Pulse.OperationClass expected, Pulse pulse)
            {
                List<Pulse.PulseTypes> possible = pulse.PossibleTypes.ToList();
                for (int i = 0; i < possible.Count; i++)
                {
                    if (Pulse.TypeToOpClass(possible[i]) != expected)
                    {
                        possible.RemoveAt(i--);
                    }
                }
                if (possible.Count == 1)
                {
                    return possible[0];
                }
                return Pulse.PulseTypes.OWErrorType;
            }

            //Public

            public static uint ParseMasterCommand(ref List<Pulse> pulses, uint start)
            {
                uint res = 0;
                for (int i = 0; i < 8; i++)
                {
                    res |= Convert.ToUInt32(pulses[(int)(i + start)].Type == Pulse.PulseTypes.OWWriteOne) << i;
                }
                return res;
            }

            //Properties
            public Slave CurrentSlave
            {
                get { return _slave; }
                set
                {
                    _slave = value;
                    Compute();
                }
            }
            public List<Pulse> Pulses
            {
                get { return _pulses; }
                set
                {
                    _pulses = value;
                    Compute();
                }
            }
            public List<Transaction> Transactions
            {
                get { return _trans; }
            }
        }

        static ulong Decode(ref List<OneWire.Pulse> pulses, uint start)
        {
            ulong res = 0;
            for (uint i = 0; i < 8; i++)
            {
                res |= Convert.ToUInt64(pulses[(int)(start + i)].Type == OneWire.Pulse.PulseTypes.OWWriteOne) << (int)(56U + i);
            }
            start += 8;
            for (uint i = 0; i < 48; i++)
            {
                res |= Convert.ToUInt64(pulses[(int)(start + i)].Type == OneWire.Pulse.PulseTypes.OWWriteOne) << (int)(8U + i);
            }
            start += 48;
            for (uint i = 0; i < 8; i++)
            {
                res |= Convert.ToUInt64(pulses[(int)(start + i)].Type == OneWire.Pulse.PulseTypes.OWWriteOne) << (int)i;
            }
            return res;
        }

        static void BackupAndOverwrite(string original_path, string new_contents)
        {
            string new_path = original_path.Replace(Path.GetFileName(original_path),
                            Path.GetFileNameWithoutExtension(original_path) + "_backup" + Path.GetExtension(original_path));
            if (!File.Exists(new_path))
            {
                File.WriteAllText(new_path, File.ReadAllText(original_path));
            }
            File.WriteAllText(original_path, new_contents);
        }

        static int Parse(string[] args)
        {
            Console.WriteLine("Preparing data...");
            string text = File.ReadAllText(args[0]).Trim('\n').Trim('\r');
            //Handle device verbose output
            int header_index = text.IndexOf('\r');
            if (!text.Substring(0, header_index).Contains(':'))
            {
                text = text.Remove(0, header_index + 1);
                BackupAndOverwrite(args[0], text);
            }
            //Handle multiple captures
            if (text.Contains("EOF"))            
            {
                string[] files = text.Split(new string[] { "EOF" }, StringSplitOptions.RemoveEmptyEntries);
                if (files.Length > 1)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        File.WriteAllText(args[0].Replace(Path.GetFileName(args[0]),
                            Path.GetFileNameWithoutExtension(args[0]) + "_splitted" + (i + 1).ToString() + Path.GetExtension(args[0])),
                            files[i]);
                    }
                    if (args.Contains("\\i:"))
                    {
                        text = files[int.Parse(args.Where(x => x.Contains("\\i:")).First()) - 1];
                    }
                    else
                    {
                        Console.WriteLine("File contained multiple captures and was split (code 3).");
                        return 3;
                    }
                }
                else
                {
                    text = files[0];
                    BackupAndOverwrite(args[0], text);
                }
            }
            //Handle single capture
            string[] lines = text.Split('\r').Select(x => x.Trim('\n')).ToArray();
            //Add additional points for the data to be plotted properly in any software (e.g. in excel) and write .csv file
            List<KeyValuePair<uint, bool>> data = new List<KeyValuePair<uint, bool>>(lines.Length * 2);
            string[] s = lines[0].Split(':');
            data.Add(new KeyValuePair<uint, bool>(uint.Parse(s[0]), uint.Parse(s[1]) > 0));
            for (int i = 1; i < lines.Length; i++) //Max 2560 lines
            {
                if (lines[i] != "")
                {
                    s = lines[i].Split(':');
                    data.Add(new KeyValuePair<uint, bool>(uint.Parse(s[0]), uint.Parse(s[1]) > 0));
                    data.Insert(data.Count - 1, new KeyValuePair<uint, bool>(data.Last().Key - 1, data[data.Count - 2].Value));
                }
            }
            if (args.Contains("/csv"))
            {
                lines = new string[data.Count];
                char c = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "," ? ';' : ',';
                for (int i = 0; i < data.Count; i++) lines[i] = data[i].Key.ToString() + c + Convert.ToInt32(data[i].Value).ToString();
                Console.WriteLine("Writing prepared data to a new file...");
                File.WriteAllLines(args[0].Replace(Path.GetExtension(args[0]), ".csv"), lines);
            }  
            Console.WriteLine("Parsing data...");
            //Parse 1-Wire pulses
            List<OneWire.Pulse> pulses = new List<OneWire.Pulse>(data.Count);
            for (int i = 1; i < data.Count; i++)
            {
                if (!data[i].Value && !data[i - 1].Value)   //If we've got a low pulse (two consecutive low points)
                {
                    pulses.Add(new OneWire.Pulse(data[i - 1].Key, data[i].Key - data[i - 1].Key));
                    if (pulses.Last().Duration == 0) pulses.Last().Duration = 1;  //Possible zeros are imperfections of the logic sniffer
                }
            }
            OneWire.CommandRow<OneWire.Pulse.OperationClass> op_class =
                new OneWire.CommandRow<OneWire.Pulse.OperationClass>
                (
                    OneWire.Pulse.OperationClass.OWReadWrite, 64
                );
            OneWire.CommandRow<OneWire.Pulse.PulseTypes> p_types = new OneWire.CommandRow<OneWire.Pulse.PulseTypes>();
            OneWire OWC = new OneWire(ref pulses, new OneWire.Slave(ref op_class, ref p_types, Decode));
            //Use everything parsed by OWC (transactions):
            Console.WriteLine("Writing parsed data to a new file...");
            string new_text = "";
            for (int i = 0; i < OWC.Transactions.Count; i++)
            {
                new_text += '#' + (i + 1).ToString() + " @ " + OWC.Transactions[i].Pulses[0].Time.ToString() + " > ";
                if (OWC.Transactions[i].DataParsable.Key)
                {
                    new_text += "M: " + OWC.Transactions[i].HEXMaster + ", ";
                }
                if (OWC.Transactions[i].DataParsable.Value)
                {
                    new_text += "S: " + OWC.Transactions[i].HEXSlave + ", ";
                }
                if (OWC.Transactions[i].Error != OneWire.Transaction.Errors.TRNoErorrs)
                {
                    new_text += "E: " + OneWire.Transaction.ErrorName(OWC.Transactions[i].Error);
                }
                new_text = new_text.Trim(',', ' ');
                if (OWC.Transactions[i].InnerErrors.InitialRow.Length > 1)
                {
                    new_text += " =>";
                    for (uint j = 0; j < OWC.Transactions[i].InnerErrors.Length; j++)
                    {
                        if (OWC.Transactions[i].InnerErrors[j] != OneWire.Transaction.Errors.TRNoErorrs)
                        {
                            new_text += Environment.NewLine + '\t' + OneWire.Transaction.ErrorName(OWC.Transactions[i].InnerErrors[j]);
                            new_text += " @ " + (OWC.Transactions[i].Pulses[j].Time).ToString();
                        }
                    }
                }
                new_text += Environment.NewLine;
            }
            new_text += "L:";
            for (int i = 0; i < OWC.Pulses.Count; i++)
            {
                new_text += Environment.NewLine + OneWire.Pulse.GetTypeName(OWC.Pulses[i].Type, true);
                new_text += " = " + OWC.Pulses[i].Duration + " @ " + OWC.Pulses[i].Time;
            }
            File.WriteAllText(args[0].Replace(Path.GetFileName(args[0]),
                Path.GetFileNameWithoutExtension(args[0]) + "_parsed" + Path.GetExtension(args[0])), new_text);
            return 0;
        }

        static int Main(string[] args)
        {
Start:
            if (args.Length < 1)
            {
                if (args.Contains("/s"))
                {
                    return 1;
                }
                Console.WriteLine("No valid path specified. Please enter data file path:");
                args = new string[] { Console.ReadLine() };
            }
            args[0] = args[0].Trim('"');
            if (!File.Exists(args[0]))
            {
                if (args.Contains("/s"))
                {
                    return 1;
                }
                args = new string[] { };
                goto Start;
            }
            //Parse data
            try
            {
                int res = Parse(args);
                if (res != 0)
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 2; 
            }
            if (!args.Contains("/s"))
            {
                Console.WriteLine("Finished. Press any key to exit...");
                Console.ReadKey();
            }
            return 0;
        }
    }
}
