﻿using System;
using VacationManagerBackend.Enum;

namespace VacationManagerBackend.Models
{
    public class VacationRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Duration { get; set; }
        public VacationRequestState State { get; set; }
        public string Annotation { get; set; }
    }
}