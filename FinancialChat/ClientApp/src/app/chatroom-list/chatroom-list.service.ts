import { Injectable } from '@angular/core';
// core
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// services
import { AppConfig } from '../app-config/app.config';

// models
import { IChatroom } from './chatroom.model';

@Injectable({
    providedIn: 'root'
})
export class ChatroomListService {
    constructor(private readonly http: HttpClient) { }

    public getChatrooms(): Observable<IChatroom[]> {
        return this.http.get<IChatroom[]>(AppConfig.getPath('chatroom', 'list'));
    }
}
