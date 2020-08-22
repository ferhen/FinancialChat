// core
import { Observable } from 'rxjs';

// imports
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from '@aspnet/signalr';

// services
import { AppConfig } from '../app-config/app.config';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

// models
import { IChatroom } from '../chatroom-list/chatroom.model';
import { IMessage } from './message.model';

@Injectable()
export class ChatroomService {
    public chatroomInfo: Observable<IChatroom>;
    public lastMessages: Observable<IMessage[]>;
    public message: Observable<IMessage>;
    private hub: HubConnection;

    constructor(private readonly authorizeService: AuthorizeService) { }

    public async initHub(): Promise<void> {
        this.buildHub();
        this.setSubscriptions();
        await this.hub.start();
    }

    public async setChatroom(chatroomId: string): Promise<void> {
        try {
            await this.hub.invoke('SetChatroom', chatroomId);
        } catch (err) {
            console.error(err);
        }
    }

    public async sendMessage(message: string, chatroomId: string): Promise<void> {
        try {
            await this.hub.invoke('ReceiveMessage', message, chatroomId);
        } catch (err) {
            console.error(err);
        }
    }

    public closeConnection(): void {
        this.hub.stop();
    }

    private buildHub(): void {
        const options: IHttpConnectionOptions  = {
            accessTokenFactory: async () => await this.authorizeService.getAccessToken().toPromise()
        };
        this.hub = new HubConnectionBuilder().withUrl(AppConfig.getPath('hubs', 'chatroomHub'), options).build();
    }

    private setSubscriptions(): void {
        this.chatroomInfo = new Observable(obs => {
            this.hub.on('ChatroomInfo', request => obs.next(request));
        });
        this.lastMessages = new Observable(obs => {
            this.hub.on('LastMessages', request => obs.next(request));
        });
        this.message = new Observable(obs => {
            this.hub.on('SendMessage', request => obs.next(request));
        });
    }
}
