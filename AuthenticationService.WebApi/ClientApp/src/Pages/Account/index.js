import React, {Fragment} from 'react';
import {Route} from 'react-router-dom';

// USER PAGES

import LoginPage from './Login/';
import RegisterPage from './Register/';
import RecoverPage from './Recover/';

const AccountPages = ({match}) => (
    <Fragment>
        <div className="app-container">
            <Route path={`/login`} component={LoginPage}/>
            <Route path={`/register`} component={RegisterPage}/>
            <Route path={`/recover`} component={RecoverPage}/>
        </div>
    </Fragment>
);

export default AccountPages;