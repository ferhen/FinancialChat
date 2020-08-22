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
        return this.http.get<IChatroom[]>(AppConfig.getPath('api', 'chatroom', 'list'));
    }

    public createChatroom(chatroomName: string): Observable<IChatroom> {
        return this.http.post<IChatroom>(
            AppConfig.getPath('api', 'chatroom', 'create'),
            { name: chatroomName }
        );
    }

    public editChatroom(chatroom: IChatroom): Observable<IChatroom> {
        return this.http.put<IChatroom>(AppConfig.getPath('api', 'chatroom', 'update'), chatroom);
    }

    public deleteChatroom(chatroomId: number): Observable<number> {
        return this.http.delete<number>(AppConfig.getPath('api', 'chatroom', 'delete') + chatroomId);
    }
}
