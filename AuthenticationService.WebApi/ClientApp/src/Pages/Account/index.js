import React, {Fragment} from 'react';
import {Route} from 'react-router-dom';

// USER PAGES

import LoginPage from './Login/';
//import LoginBoxed from './LoginBoxed/';

import RegisterPage from './Register/';
// import RegisterBoxed from './RegisterBoxed/';

import RecoverPage from './Recover/';
// import ForgotPasswordBoxed from './ForgotPasswordBoxed/';

const AccountPages = ({match}) => (
    <Fragment>
        <div className="app-container">

            {/* User Pages */}

            <Route path={`/login`} component={LoginPage}/>
            <Route path={`/register`} component={RegisterPage}/>
            <Route path={`/recover`} component={RecoverPage}/>

            {/* <Route path={`${match.url}/login-boxed`} component={LoginBoxed}/>
            <Route path={`${match.url}/register`} component={Register}/>
            <Route path={`${match.url}/register-boxed`} component={RegisterBoxed}/>
            <Route path={`${match.url}/forgot-password`} component={ForgotPassword}/>
            <Route path={`${match.url}/forgot-password-boxed`} component={ForgotPasswordBoxed}/> */}
        </div>
    </Fragment>
);

export default AccountPages;