using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Core
{
    public interface IInteractor<out TResult>
    {
        Task<TViewModel> HandleAsync<TViewModel>(IPresenter<TViewModel, TResult> presenter);
    }

    public interface IInteractor<out TResult, in T>
    {
        Task<TViewModel> HandleAsync<TViewModel>(IPresenter<TViewModel, TResult> presenter, T input);
    }

    public interface IInteractor<out TResult, in T1, in T2>
    {
        Task<TViewModel> HandleAsync<TViewModel>(IPresenter<TViewModel, TResult> presenter, T1 input, T2 input2);
    }

    public interface IInteractor<out TResult, in T1, in T2, in T3>
    {
        Task<TViewModel> HandleAsync<TViewModel>(IPresenter<TViewModel, TResult> presenter, T1 input, T2 input2, T3 input3);
    }

    public interface IInteractor<out TResult, in T1, in T2, in T3, in T4>
    {
        Task<TViewModel> HandleAsync<TViewModel>(IPresenter<TViewModel, TResult> presenter, T1 input, T2 input2, T3 input3, T4 input4);

    }
}
