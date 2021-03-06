﻿import 'isomorphic-fetch';

import WebsocketService from './WebSocketService';
import { User } from '../services/Models/User';

export class UsersService {
    private _userLoggedOn: (user: User) => void;

    constructor(socketCallback: (user: User) => void) {
        this._userLoggedOn = socketCallback;

        WebsocketService.registerUserLoggedOn((user: User) => {
            this._userLoggedOn(user);
        });
    }

    public fetchLogedOnUsers(fetchUsersCallback: (msg: User[]) => void) {
        fetch('api/Chat/LoggedOnUsers')
            .then(response => response.json() as Promise<User[]>)
            .then(data => { fetchUsersCallback(data) });
    }
}
