import React, {Component, Fragment} from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';

import { ClientGateway } from '../../../Services/ClientGateway';

import PageTitle from '../../../Layout/AppMain/PageTitle';

import {
    Row, Col,
    //Button,
    //CardHeader,
    Card,
    //CardBody,
    //Progress,
    //TabContent,
    //TabPane,
} from 'reactstrap';

class ClientRow extends Component {
    constructor(props) {
        super(props);

        this.state = {
            client: this.props.client,
            index: this.props.index
        }
    }

    render() {
        return (
            <tr>
                <td className="text-center text-muted">{this.state.index}</td>
                <td>
                    {this.state.client.clientId}
                </td>
                <td>
                    {this.state.client.clientName}
                </td>
                {/* <td>
                    <div className="widget-content p-0">
                        <div className="widget-content-wrapper">
                            <div className="widget-content-left mr-3">
                                <div className="widget-content-left">
                                    
                                </div>
                            </div>
                            <div className="widget-content-left flex2">
                                <div className="widget-heading">{this.state.client.clientName}</div>
                                <div className="widget-subheading opacity-7">{this.state.client.clientId}</div>
                            </div>
                        </div>
                    </div>
                </td> */}
                <td>{this.state.client.description}</td>
                <td className="text-center">
                    {this.state.client.enabled
                        ? <div className="badge badge-success"> {String(this.state.client.enabled)}</div>
                        : <div className="badge badge-danger"> {String(this.state.client.enabled)}</div>
                    }
                </td>
                <td className="text-center">
                    <button type="button" className="btn btn-primary btn-sm">Details</button>
                </td>
            </tr>
        );
    }
}

export default class ListClients extends Component {
    constructor(props){
        super(props);
        this.state = {
            clients: null
        }
    }

    componentDidMount(){
        this.getClients();
    }

    getClients() {
        ClientGateway.getClients()
            .then(result => {
                if(result && Array.isArray(result)) {
                    //self.state.clients = result;
                    this.setState({clients: result});
                }
            });
    }

    render() {
        return (
        <Fragment>
                <ReactCSSTransitionGroup
                    component="div"
                    transitionName="TabsAnimation"
                    transitionAppear={true}
                    transitionAppearTimeout={0}
                    transitionEnter={false}
                    transitionLeave={false}>
                    <div>
                        <PageTitle
                            heading="Auth clients"
                            subheading="Display authenticate service associated client applications/services"
                            icon="pe-7s-albums icon-gradient bg-mean-fruit"
                        />
                        <Row>
                            <Col md="12">
                                <Card className="main-card mb-3">
                                    <div className="card-header">Clients
                                        <div className="btn-actions-pane-right">
                                            {/*
                                            <div role="group" className="btn-group-sm btn-group">
                                                <button className="active btn btn-info">Last Week</button>
                                                <button className="btn btn-info">All Month</button>
                                            </div>
                                            */}
                                        </div>
                                    </div>
                                    <div className="table-responsive">
                                        <table className="align-middle mb-0 table table-borderless table-striped table-hover">
                                            <thead>
                                            <tr>
                                                <th className="text-center">#</th>
                                                <th>ClientId</th>
                                                <th>Name </th>
                                                <th>Description</th>
                                                <th className="text-center">State</th>
                                                <th className="text-center">Actions</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            {!this.state.clients && (<tr><td className="text-center" colSpan="5">No clients associated!</td></tr>)}
                                            {this.state.clients && this.state.clients.map((client, index) => <ClientRow client={client} index={index} key={index} />)}
                                            </tbody>
                                        </table>
                                    </div>
                                    <div className="d-block text-center card-footer">
                                        {/*<button className="mr-2 btn-icon btn-icon-only btn btn-outline-danger"><i className="pe-7s-trash btn-icon-wrapper"></i>  Terminate all sessions</button>
                                         <button className="btn-wide btn btn-success">Save</button> */}
                                    </div>
                                </Card>
                            </Col>
                        </Row>
                    </div>
                </ReactCSSTransitionGroup>
            </Fragment>
        );
    }
}
