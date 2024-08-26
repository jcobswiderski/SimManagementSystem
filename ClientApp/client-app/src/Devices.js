import config from './config';
import { Component } from "react";

class Devices extends Component {
    API_URL = "http://localhost:5272";

    constructor(props) {
        super(props);
        this.state = {
            devices: []
        }
    }

    componentDidMount() {
        this.refreshDevices();
    }

    async refreshDevices() {
        fetch(`${config.API_URL}/Devices`)
            .then(res => res.json())
            .then(data => this.setState({devices: data}));
    }

    async addClick() {
        var newDevice = document.querySelector('#newDevice').value;
        const data = new FormData();
        data.append("newDevice", newDevice);

        fetch(`${config.API_URL}/Devices`, {
            method: "POST",
            body: data
        }).then(res => res.json()).then((result) => {
            alert(result);
            this.refreshDevices();
        })
        
    }

    async deleteClick(id) {
        fetch(`${config.API_URL}/Devices` + id, {
            method: "DELETE"
        }).then(res => {
            if(res.ok) {
                console.log("GYT");
            }
        })
        
    }

    render() {
        const {devices} = this.state; 

        return (
            <div className="devices">
                <h1 className="devices__title">Devices</h1>
                <div className="devices__group">
                    <input className="devices__input" type="text" id="newDevice" />
                    <button className="devices__button devices__button--add" onClick={() => this.addClick()}>Add device</button>
                </div>
                {devices.map(device => 
                    <div className="devices__card" key={device.id}>
                        <h2 className="devices__card__title">{device.name}</h2>
                        <button className="devices__button devices__button--modify">Modify</button>
                        <button className="devices__button devices__button--delete" onClick={() => this.deleteClick(device.id)}>Delete</button>
                    </div>
                )}
            </div>
        );
    }
}

export default Devices;