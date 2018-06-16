using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewtonDemo
{
    public class Physics : INotifyPropertyChanged // Наследуемся от нужного интерфеса
    {
        public event PropertyChangedEventHandler PropertyChanged; // Событие, которое нужно вызывать при изменении

        // Для удобства обернем событие в метод с единственным параметром - имя изменяемого свойства
        public void RaisePropertyChanged(string propertyName)
        {
            // Если кто-то на него подписан, то вызывем его
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private double _mass = 0;
        private double _force = 0;
        private double _acceleration = 0;

        public double Acceleration { get => _acceleration; set { _acceleration = value; RaisePropertyChanged("Acceleration"); } }
        public double Force { get => _force; set { _force = value; RaisePropertyChanged("Force"); } }
        public double Mass { get => _mass; set { _mass = value; RaisePropertyChanged("Mass"); } }

        public double GetAcceleration()
        {          
            return Force / Mass;
        }

        public double GetAcceleration(double force, double mass)
        {
            return force / mass;
        }

        public double GetForce()
        {           
            return Acceleration * Mass;
        }

        public double GetForce(double acceleration, double mass)
        {
            return acceleration * Mass;
        }

        public double GetMass()
        {           
            return Force / Acceleration;
        }
    }
}
