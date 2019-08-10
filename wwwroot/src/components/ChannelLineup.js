import React, { Component } from 'react';
import { Alert, Container, Row, Col } from 'reactstrap';

import { Status } from './Status';
import { Channels } from './Channels';

export class ChannelLineup extends Component {
  constructor(props) {
    super(props);

    this.onGuideRefresh = this.onGuideRefresh.bind(this);
    this.onAlertToggle = this.onAlertToggle.bind(this);

    this.state = {
      alert: {
        visible: false,
        text: null,
        error: false
      }
    };
  }

  onGuideRefresh(success) {
    var text = success
      ? "Guide refresh requested successfully"
      : "An error occurred requesting guide refresh (check logs)";

    this.setState({
      alert: {
        visible: true,
        text: text,
        error: !success
      }
    });

    setInterval(this.onAlertToggle, 5000);
  }

  onAlertToggle() {
    this.setState({alert: {visible: false}});
  }

  render () {
    var alert = this.state.alert;
    return (
      <Container>
        <Alert isOpen={alert.visible} color={alert.error ? "danger" : "success"} toggle={this.onAlertToggle}>{alert.text}</Alert>
        <Row>
          <Col xs="4">
            <Status onRefresh={this.onGuideRefresh}/>
          </Col>
          <Col xs="8">
            <Channels/>
          </Col>
        </Row>
      </Container>
    );
  }
}
