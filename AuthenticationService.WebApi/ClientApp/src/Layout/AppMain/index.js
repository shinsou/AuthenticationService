import {BrowserRouter as Router, Route, Redirect} from 'react-router-dom';
import React, {Component, Suspense, lazy, Fragment} from 'react';
import PrivateRoute from '../../PrivateRoute';
import AppLoader from '../AppLoader'
import {
    ToastContainer,
} from 'react-toastify';


const AccountPages = lazy(() => import('../../Pages/Account'));

const Dashboards = lazy(() => import('../../Pages/Dashboards'));
const CustomersPages = lazy(() => import('../../Pages/Customers'));
const ClientsPages = lazy(() => import('../../Pages/Clients'));
const ResourcesPages = lazy(() => import('../../Pages/Resources'));
const UserPages = lazy(() => import('../../Pages/User'));

class PrivatePages extends Component {
    render() {
        return (
            <Suspense fallback={<AppLoader headerText="Please wait while loading modules..." smallText="Be patient, this shouldn't take long :)" />}>
                <PrivateRoute path="/dashboards" component={Dashboards} />
                <PrivateRoute path="/customers" component={CustomersPages} />
                <PrivateRoute path="/clients" component={ClientsPages} />
                <PrivateRoute path="/resources" component={ResourcesPages} />
                <PrivateRoute path="/users" component={UserPages} />
            </Suspense>
            );
    }
}

class PublicPages extends Component {
    render() {
        return(
            <Suspense fallback={<AppLoader headerText="Please wait while loading login modules..." smallText="Be patient, this shouldn't take long :)" />}>
                <Route path="/login" component={AccountPages} />
                <Route path="/register" component={AccountPages} />
                <Route path="/recover" component={AccountPages} />
            </Suspense>
        );
    }
}

const AppMain = () => {
    return (
        <Fragment>
            <PrivatePages />
            <PublicPages />

            <Route exact path="/" render={() => (
                <Redirect to="/dashboards/activity"/>
            )}/>
            <ToastContainer/>
        </Fragment>
    )
};

export default AppMain;