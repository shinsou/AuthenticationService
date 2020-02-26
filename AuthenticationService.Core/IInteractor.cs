﻿using System;
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
}
