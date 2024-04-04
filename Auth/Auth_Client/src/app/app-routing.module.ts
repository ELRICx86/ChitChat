import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { ChatComponent } from './chat/chat.component';
import { authGuard } from './Auth/auth.guard';
import { PendingComponent } from './pending/pending.component';
import { CommonModule } from '@angular/common';
import { FindfriendComponent } from './findfriend/findfriend.component';

const routes: Routes = [
  { path: "login", component: LoginComponent },
  { path: "add", component: FindfriendComponent },
  { path: "register", component: RegisterComponent },
  {path:"chat", component: ChatComponent, canActivate: [authGuard]},
  {path: "pending", component: PendingComponent,canActivate: [authGuard]},
  { path: "**", redirectTo: "" } // Redirect to default route (usually AppComponent)
];


@NgModule({
  imports: [RouterModule.forRoot(routes),CommonModule],
  exports: [RouterModule]
})
export class AppRoutingModule { }
