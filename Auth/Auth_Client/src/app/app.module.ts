import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { HttpClientModule, provideHttpClient, withFetch } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { RegisterComponent } from './register/register.component';
import { NavbarComponent } from './navbar/navbar.component';
import { ChatComponent } from './chat/chat.component';
import { ContactsComponent } from './contacts/contacts.component';
import { MessagesComponent } from './messages/messages.component';
import { InputComponent } from './input/input.component';
import { PendingComponent } from './pending/pending.component';
import { FriendlistComponent } from './friendlist/friendlist.component';
import { AvatarModule } from 'ngx-avatars';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { FindfriendComponent } from './findfriend/findfriend.component';
import { CarouselModule } from 'primeng/carousel';
import { ListboxModule } from 'primeng/listbox';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';



const avatarColors = ["#FFB6C1", "#2c3e50", "#95a5a6", "#f39c12", "#1abc9c"];
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    NavbarComponent,
    ChatComponent,
    ContactsComponent,
    MessagesComponent,
    InputComponent,
    PendingComponent,
    FriendlistComponent,
    FindfriendComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    FormsModule,
    AvatarModule.forRoot({
      colors: avatarColors
    }),
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      closeButton:true,
      progressBar:true,
      progressAnimation:'increasing',
      enableHtml:true,
      positionClass: 'toast-top-left',
      

    }),
    ButtonModule,
    PasswordModule,
    CarouselModule,
    ListboxModule,
    TableModule,
    InputTextModule
  ],
  providers: [
    provideClientHydration(),
    provideHttpClient(withFetch())
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
