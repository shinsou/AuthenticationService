import {BrowserRouter as Router, Route, Redirect} from 'react-router-dom';
import React, {Suspense, lazy, Fragment} from 'react';
import PrivateRoute from '../../PrivateRoute';

import {
    ToastContainer,
} from 'react-toastify';


const AccountPages = lazy(() => import('../../Pages/Account'));

const Dashboards = lazy(() => import('../../Pages/Dashboards'));
const CustomersPages = lazy(() => import('../../Pages/Customers'));
const ClientsPages = lazy(() => import('../../Pages/Clients'));
const ResourcesPages = lazy(() => import('../../Pages/Resources'));
const UserPages = lazy(() => import('../../Pages/User'));

const AppMain = () => {

    return (
        <Fragment>
            {/* Dashboards */}

            <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <h6 className="mt-3">
                            Please wait while we load dashboard...
                        </h6>
                    </div>
                </div>
            }>
                <PrivateRoute path="/dashboards" component={Dashboards} />
                <PrivateRoute path="/customers" component={CustomersPages} />
                <PrivateRoute path="/clients" component={ClientsPages} />
                <PrivateRoute path="/resources" component={ResourcesPages} />
                <PrivateRoute path="/users" component={UserPages} />
            </Suspense>
            <Suspense fallback={
                <div className="loader-container">
                    <div className="loader-container-inner">
                        <h6 className="mt-3">
                            Please wait while loading login
                            <small>Because this is a demonstration, we load at once all the Dashboards examples. This wouldn't happen in a real live app!</small>
                        </h6>
                    </div>
                </div>
                }>
                <Route path="/login" component={AccountPages} />
                <Route path="/register" component={AccountPages} />
                <Route path="/recover" component={AccountPages} />
            </Suspense>

            <Route exact path="/" render={() => (
                <Redirect to="/dashboards/activity"/>
            )}/>
            <ToastContainer/>
        </Fragment>
    )
};

export default AppMain;