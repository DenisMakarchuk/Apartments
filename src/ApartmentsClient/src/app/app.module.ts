import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

import {  ApartmentUserService,
          CommentUserService,
          OrderUserService,
          UserService,
          ApartmentSearchService,
          CommentAdministrationService,
          UserAdministrationService
        } from '../app/services/nswag.generated.service';
import { TokenInterceptorService } from './auth/token-interceptor.service';
import { LoggedService } from '../app/services/logged.service';
import { SearchParametersService } from '../app/services/search-parameters.service';

import { AppComponent } from './app.component';
import { MainMenuComponent } from './main-menu/main-menu.component';
import { LogoutComponent } from './auth/logout/logout.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { AuthGuard } from './auth/auth.guard';
import { SearchComponent } from './search/search.component';
import { SearchViewComponent } from './search/search-view/search-view.component';
import { SearchApartmentDetailComponent } from './search/search-apartment-detail/search-apartment-detail.component';
import { ProfilePanelComponent } from './profile/profile-panel/profile-panel.component';
import { OrderDetailComponent } from './user/order/order-detail/order-detail.component';
import { OwnOrdersComponent } from './user/order/own-orders/own-orders.component';


@NgModule({
  declarations: [
    AppComponent,
    MainMenuComponent,
    LogoutComponent,
    LoginComponent, 
    RegisterComponent, SearchComponent, SearchViewComponent, SearchApartmentDetailComponent, ProfilePanelComponent, OrderDetailComponent, OwnOrdersComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true
    },
    ApartmentUserService,
    CommentUserService,
    OrderUserService,
    UserService,
    ApartmentSearchService,
    CommentAdministrationService,
    UserAdministrationService,
    LoggedService,
    SearchParametersService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
