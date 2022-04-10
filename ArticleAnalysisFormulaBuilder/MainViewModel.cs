using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using PropertyChanged;

namespace ArticleAnalysisFormulaBuilder;

[AddINotifyPropertyChangedInterface]
internal class MainViewModel : BaseViewModel
{
    private readonly struct Bank
    {
        /// <summary>
        /// 符号
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; }

        public Bank(string symbol, string value)
        {
            Symbol = symbol;
            Value = value;
        }
    }

    /// <summary>
    /// 储存
    /// </summary>
    private static readonly List<Bank> Banks = new()
    {
        new Bank("a", "基期 ( A )"),
        new Bank("b", "现期 ( B )"),
        new Bank("delta", "增长量 ( Δ )"),
        new Bank("ate", "增长率 ( r )"),
    };

    private static readonly List<string> Answers = new()
    {
        "/Answers/a_b_delta.png",
        "/Answers/a_b_ate.png",
        "/Answers/a_delta_ate.png",
        "/Answers/b_a_delta.png",
        "/Answers/b_a_ate.png",
        "/Answers/b_delta_ate.png",
        "/Answers/delta_a_ate.png",
        "/Answers/delta_b_a.png",
        "/Answers/delta_b_ate.png",
        "/Answers/ate_b_a.png",
        "/Answers/ate_delta_a.png",
        "/Answers/ate_delta_b.png"
    };

    /// <summary>
    /// 所求
    /// </summary>
    [OnChangedMethod(nameof(CalculateAnswer))]
    public string Request { get; set; } = null!;

    /// <summary>
    /// 所求候选
    /// </summary>
    public List<string> Requests => Banks.Select(b => b.Value).ToList();

    /// <summary>
    /// 条件1
    /// </summary>
    [OnChangedMethod(nameof(CalculateAnswer))]
    public string Condition1 { get; set; } = null!;

    /// <summary>
    /// 条件1候选
    /// </summary>
    public List<string> Conditions1 => Banks.Where(b => b.Value != Request).Select(b => b.Value).ToList();

    /// <summary>
    /// 条件2
    /// </summary>
    [OnChangedMethod(nameof(CalculateAnswer))]
    public string Condition2 { get; set; } = null!;

    /// <summary>
    /// 条件2候选
    /// </summary>
    public List<string> Conditions2 =>
        Banks.Where(b => b.Value != Request && b.Value != Condition1).Select(b => b.Value).ToList();

    /// <summary>
    /// 答案
    /// </summary>
    public string Answer { get; set; } = null!;

    /// <summary>
    /// 生成问题
    /// </summary>
    public ICommand BuildCommand => new Command(BuildQuestion);

    /// <summary>
    /// 生成问题
    /// </summary>
    private void BuildQuestion()
    {
        var next = new Random().Next(Banks.Count);
        var selected = Banks[next];

        // 选剩下的3个
        var remain = Banks.Where(v => v.Symbol != selected.Symbol).ToList();

        // 3个中选1个排除(也就是3个选2个)
        var exclude = remain[new Random().Next(remain.Count)];
        var conditions = remain.Where(r => r.Symbol != exclude.Symbol).ToList();

        Request = selected.Value;
        Condition1 = conditions[0].Value;
        Condition2 = conditions[1].Value;
        Answer = Answers.Where(s => s.TrimStart("/Answers/".ToArray()).Split('_').First() == selected.Symbol)
            .First(s => s.Contains(conditions[0].Symbol) && s.Contains(conditions[1].Symbol));
        _firstLoad = false;
    }

    private bool _firstLoad = true;

    private void CalculateAnswer()
    {
        if (_firstLoad) return;
        if (string.IsNullOrWhiteSpace(Request) ||
            string.IsNullOrWhiteSpace(Condition1) ||
            string.IsNullOrWhiteSpace(Condition2))
            return;

        var request = Banks.First(b => b.Value == Request);
        var condition1 = Banks.First(b => b.Value == Condition1);
        var condition2 = Banks.First(b => b.Value == Condition2);
        Answer = Answers.Where(s => s.TrimStart("/Answers/".ToArray()).Split('_').First() == request.Symbol)
            .First(s => s.Contains(condition1.Symbol) && s.Contains(condition2.Symbol));
    }

    public MainViewModel()
    {
        BuildQuestion();
    }
}