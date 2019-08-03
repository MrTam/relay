import React, { Component } from 'react';
import { Card, CardBody, CardTitle, CardText, Table } from 'reactstrap';

export class TunerStatus extends Component {
    constructor(props) {
        super(props);
        this.state = { device: {}};
    }

    componentDidMount() {
        fetch('discover.json')
            .then(resp => resp.json())
            .then(data => this.setState({device: data}));
    }

    render() {
        let device = this.state.device;

        return (
            <div>
                <Card>
                    <CardBody>
                        <CardTitle>Status</CardTitle>
                        <CardText>
                            <Table striped>
                                <tbody>
                                    <tr>
                                        <th>Provider</th>
                                    </tr>
                                    <tr>
                                        <th>Refresh</th>
                                    </tr>
                                    <tr>
                                        <th>Tuners</th>
                                        <td>{device.tunerCount}</td>
                                    </tr>
                                </tbody>
                            </Table>
                        </CardText>
                    </CardBody>
                </Card>
            </div>
        );

    }
}