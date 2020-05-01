import { Component, OnInit } from '@angular/core';

import { ApartmentUserService } from 'src/app/core/nswag.generated.service';
import { ApartmentView } from 'src/app/core/nswag.generated.service';
import { AddApartment } from 'src/app/core/nswag.generated.service';
import { ApartmentDTO } from 'src/app/core/nswag.generated.service';

import { CommentUserService } from 'src/app/core/nswag.generated.service';
import { AddComment } from 'src/app/core/nswag.generated.service';
import { CommentDTO } from 'src/app/core/nswag.generated.service';

import { OrderUserService } from 'src/app/core/nswag.generated.service';
import { AddOrder } from 'src/app/core/nswag.generated.service';
import { OrderView } from 'src/app/core/nswag.generated.service';
import { OrderDTO } from 'src/app/core/nswag.generated.service';




@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
