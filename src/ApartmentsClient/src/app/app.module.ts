import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ApartmentSearchService } from './core/nswag.generated.service';
import { UserService } from 'src/app/core/nswag.generated.service';

import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { ApartmentSearchComponent } from './apartment-search/apartment-search.component';
import { ProfileComponent } from './profile/profile.component';
import { LoginComponent } from './auth/login/login.component';
import { OrderDetailComponent } from './userCapabilities/order-detail/order-detail.component';
import { ApartmentDetailComponent } from './userCapabilities/apartment-detail/apartment-detail.component';
import { CommentDetailComponent } from './userCapabilities/comment-detail/comment-detail.component';
import { RegisterComponent } from './auth/register/register.component';
import { LogoutComponent } from './auth/logout/logout.component';
import { AuthGuard } from './auth/auth.guard';
import { OwnApartmentsComponent } from './own/own-apartments/own-apartments.component';
import { OwnOrdersComponent } from './own/own-orders/own-orders.component';


@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    ApartmentSearchComponent,
    ProfileComponent,
    LoginComponent,
    OrderDetailComponent,
    ApartmentDetailComponent,
    CommentDetailComponent,
    RegisterComponent,
    LogoutComponent,
    OwnApartmentsComponent,
    OwnOrdersComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    ApartmentSearchService,
    AuthGuard,
    UserService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
