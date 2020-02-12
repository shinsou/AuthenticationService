import React, {Fragment} from 'react';
import {Route} from 'react-router-dom';

import ListUsers from './List/';
import CreateUser from './Create/';

// Layout

import AppHeader from '../../Layout/AppHeader/';
import AppSidebar from '../../Layout/AppSidebar/';
import AppFooter from '../../Layout/AppFooter/';

const UserPages = ({match}) => (
    <Fragment>
        <AppHeader/>
        <div className="app-main">
            <AppSidebar/>
            <div className="app-main__outer">
                <div className="app-main__inner">
                    <Route path={`${match.url}/list`} component={ListUsers}/>
                    <Route path={`${match.url}/create`} component={CreateUser}/>
                </div>
                <AppFooter/>
            </div>
        </div>
    </Fragment>
);

export default UserPages;