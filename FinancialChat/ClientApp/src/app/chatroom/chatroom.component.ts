// core
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, BehaviorSubject, Subject } from 'rxjs';

// services
import { ChatroomService } from './chatroom.service';

// models
import { IChatroom } from '../chatroom-list/chatroom.model';
import { IMessage } from './message.model';
import { takeUntil } from 'rxjs/operators';

@Component({
    templateUrl: './chatroom.component.html',
    styleUrls: ['./chatroom.component.css'],
    providers: [ChatroomService]
})
export class ChatroomComponent implements OnInit, OnDestroy {
    private chatroomId: string;

    public chatroomInfo: Observable<IChatroom>;
    public readonly messages = new BehaviorSubject<IMessage[]>([]);
    private readonly destroy$ = new Subject();

    public inputMessage = '';

    constructor(private readonly service: ChatroomService,
        private readonly route: ActivatedRoute) { }

    public async sendMessage(): Promise<void> {
        if (!this.inputMessage) {
            return;
        }
        try {
            await this.service.sendMessage(this.inputMessage, this.chatroomId);
            this.inputMessage = '';
        } catch (err) {
            console.error(err);
        }
    }

    private async connect(chatroomId: string): Promise<void> {
        try {
            await this.service.initHub();
            this.subscribeToObservables();
            await this.service.setChatroom(chatroomId);
        } catch (err) {
            console.error(err);
        }
    }

    private subscribeToObservables(): void {
        this.chatroomInfo = this.service.chatroomInfo;

        this.service.lastMessages
            .pipe(takeUntil(this.destroy$))
            .subscribe(messages => {
                this.messages.next(messages);
                this.scrollToBottom();
            });

        this.service.message
            .pipe(takeUntil(this.destroy$))
            .subscribe(message => {
                this.messages.next([...this.messages.getValue(), message].slice(-50));
                this.scrollToBottom();
            });
    }

    private scrollToBottom(): void {
        setTimeout(() => {
            const messageContainer = document.querySelector('.message-container');
            messageContainer.scroll(0, messageContainer.scrollHeight);
        });
    }

    ngOnInit(): void {
        this.chatroomId = this.route.snapshot.params.id;
        this.connect(this.chatroomId);
    }

    ngOnDestroy() {
        this.destroy$.next();
        this.destroy$.complete();
        this.service.closeConnection();
    }
}
