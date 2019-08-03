import React, { Component } from 'react';
import update from 'react-addons-update';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faHeart as heartSolid } from '@fortawesome/free-solid-svg-icons'
import { faHeart as heartRegular } from '@fortawesome/free-regular-svg-icons'

import { Card, CardBody, CardTitle,
    CardSubtitle, CardText, Table } from 'reactstrap';

import './Channels.css'

export class Channels extends Component {
    constructor(props) {
        super(props);
        this.state = { channels: [] };
    }

    componentDidMount() {
        fetch('lineup.json')
            .then(resp => resp.json())
            .then(data => this.setState({channels: data}));
    }

    toggleFavourite(channel, isFavourite) {
         fetch(`lineup.post/?favorite=${isFavourite ? '-' : '+'}${channel}`, {
             method: 'POST', headers: { 'Content-Type': 'application/json' }, body: {}})

             .then(resp => {
                if(resp.status === 200) {
                    var index = this.state.channels.findIndex(c => c.GuideNumber === channel);

                     this.setState({
                         channels: update(this.state.channels, {[index]: {Favorite: {$set: !isFavourite}}})
                     });
                }
                // TODO: Add a toast on failure
             });
    }

    render () {
        let channels = this.state.channels;
        var hdChannelCount = channels.filter(c => c.HD).length;
        var favCount = channels.filter(c => c.Favorite).length;

        return (
            <div>
                <Card>
                    <CardBody>
                        <CardTitle>Channel Lineup</CardTitle>
                        <CardSubtitle>{channels.length} Channels (HD: {hdChannelCount}, Favourites: {favCount})</CardSubtitle>
                        <CardText>
                            <Table striped hover borderless>
                                <thead>
                                    <tr>
                                        <th scope="col">#</th>
                                        <th scope="col" />
                                        <th scope="col" className="col-8">Name</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {channels.map(channel =>
                                        <tr>
                                            <th scope="row">{channel.GuideNumber}</th>
                                            <td>
                                                <span id="heart-icon" onClick={e => this.toggleFavourite(channel.GuideNumber, channel.Favorite)}>
                                                    <FontAwesomeIcon icon={channel.Favorite ? heartSolid : heartRegular}/>
                                                </span>
                                            </td>
                                            <td>
                                                {channel.GuideName} 
                                                {channel.HD ? <span className="badge badge-pill badge-dark">HD</span>: ""}
                                            </td>
                                        </tr>
                                    )}
                                </tbody>
                            </Table>
                        </CardText>
                    </CardBody>
                </Card>
            </div>
        )
    }
}
