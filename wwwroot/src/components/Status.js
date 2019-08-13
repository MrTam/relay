import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Button, Card, CardBody, CardTitle, Table } from 'reactstrap';

export class Status extends Component {
    constructor(props) {
        super(props);
        this.state = { device: {}};
        this.onRefresh = this.onRefresh.bind(this);
    }

    componentDidMount() {
        fetch('status.json')
            .then(resp => resp.json())
            .then(data => this.setState({device: data}));
    }

    onRefresh() {
        fetch('lineup.update', {method: "POST"})
            .then(res => this.props.onRefresh(res.status === 200));
    }

    render() {
        let device = this.state.device;

        return (
            <div>
                <Card>
                    <CardBody>
                        <CardTitle>Status</CardTitle>
                            <Table striped>
                                <tbody>
                                    <tr>
                                        <th>Provider</th>
                                        <td><a href={device.providerUrl}>{device.provider}</a></td>
                                    </tr>
                                    <tr>
                                        <th>Refresh</th>
                                        <td>{device.refreshInterval}</td>
                                    </tr>
                                    <tr>
                                        <th>Tuners</th>
                                        <td>{device.tuners}</td>
                                    </tr>
                                </tbody>
                            </Table>
                            <Button onClick={this.onRefresh}>Update Guide</Button>
                    </CardBody>
                </Card>
            </div>
        );
    }
}

Status.propTypes = {
    onRefresh: PropTypes.func
}

Status.defaultProps = {
    onRefresh: success => {}
}