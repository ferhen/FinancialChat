// core
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// imports
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

// app config
import { AppConfig } from './app-config/app.config';

// components
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ChatroomListComponent } from './chatroom-list/chatroom-list.component';

// authorization
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { ChatroomComponent } from './chatroom/chatroom.component';

// modals
import { DeleteChatroomModalComponent } from './chatroom-list/delete-chatroom-modal/delete-chatroom-modal.component';
import { CreateChatroomModalComponent } from './chatroom-list/create-chatroom-modal/create-chatroom-modal.component';

export function initializeApp(appConfig: AppConfig) {
  return () => appConfig.load();
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ChatroomListComponent,
    ChatroomComponent,
    DeleteChatroomModalComponent,
    CreateChatroomModalComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'chatroom-list', component: ChatroomListComponent, canActivate: [AuthorizeGuard] },
      { path: 'chatroom/:id', component: ChatroomComponent, canActivate: [AuthorizeGuard] },
    ]),
    NgbModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AppConfig],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizeInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
