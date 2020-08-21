// core
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// models
import { IChatroom } from '../chatroom-list/chatroom.model';

@Injectable({
    providedIn: 'root'
})
export class ChatroomService {
    constructor(private readonly http: HttpClient) { }

    public getChatroomInfo(id: number): Observable<IChatroom> {
        return this.http.get<IChatroom>('' + id);
    }
}
