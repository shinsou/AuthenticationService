import React, {Fragment} from 'react';
import {Route} from 'react-router-dom';

// List

import ListCustomers from './List';
import CreateCustomer from './Create';

// Layout

import AppHeader from '../../Layout/AppHeader/';
import AppSidebar from '../../Layout/AppSidebar/';
import AppFooter from '../../Layout/AppFooter/';

const Clients = ({match}) => (
    <Fragment>
        <AppHeader/>
        <div className="app-main">
            <AppSidebar/>
            <div className="app-main__outer">
                <div className="app-main__inner">
                    <Route path={`${match.url}/list`} component={ListCustomers}/>
                    <Route path={`${match.url}/create`} component={CreateCustomer}/>
                </div>
                <AppFooter/>
            </div>
        </div>
    </Fragment>
);

export default Clients;